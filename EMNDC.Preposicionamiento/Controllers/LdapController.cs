using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Dto;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.Protocols;
using System.Net;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class LdapController : ControllerBase
    {
        private readonly ILdapConfigService _service;
        private readonly PreposicionamientoDbContext _context;
        private readonly IAuditService _audit;

        public LdapController(ILdapConfigService service, PreposicionamientoDbContext context, IAuditService audit)
        {
            _service = service;
            _context = context;
            _audit = audit;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cfg = await _service.GetAsync();
            string? email = null;
            if (!string.IsNullOrEmpty(cfg.UpdatedByUserId))
            {
                email = await _context.Users
                    .Where(u => u.Id == cfg.UpdatedByUserId)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();
            }

            return Ok(new LdapConfigurationDto
            {
                Enabled = cfg.Enabled,
                Host = cfg.Host,
                Port = cfg.Port,
                UseSsl = cfg.UseSsl,
                BaseDn = cfg.BaseDn,
                BindDn = cfg.BindDn,
                HasPassword = !string.IsNullOrEmpty(cfg.BindPasswordEncrypted),
                UserSearchFilter = cfg.UserSearchFilter,
                EmailAttribute = cfg.EmailAttribute,
                NameAttribute = cfg.NameAttribute,
                LastNameAttribute = cfg.LastNameAttribute,
                DefaultRole = cfg.DefaultRole,
                UpdatedAt = cfg.UpdatedAt,
                UpdatedByUserId = cfg.UpdatedByUserId,
                UpdatedByEmail = email
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LdapConfigurationUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Host))
                return BadRequest("Host requerido.");
            if (dto.Port <= 0 || dto.Port > 65535)
                return BadRequest("Port inválido.");

            var userId = User.GetUserIdFromToken();

            await _service.UpdateAsync(new LdapConfiguration
            {
                Enabled = dto.Enabled,
                Host = dto.Host.Trim(),
                Port = dto.Port,
                UseSsl = dto.UseSsl,
                BaseDn = dto.BaseDn?.Trim() ?? string.Empty,
                BindDn = dto.BindDn?.Trim() ?? string.Empty,
                UserSearchFilter = dto.UserSearchFilter?.Trim() ?? string.Empty,
                EmailAttribute = dto.EmailAttribute?.Trim() ?? "mail",
                NameAttribute = dto.NameAttribute?.Trim() ?? "givenName",
                LastNameAttribute = dto.LastNameAttribute?.Trim() ?? "sn",
                DefaultRole = dto.DefaultRole?.Trim() ?? "User",
            }, dto.BindPassword, userId);

            await _audit.LogAsync(AuditActions.LdapConfigUpdated,
                description: $"Configuración LDAP actualizada (host={dto.Host}, enabled={dto.Enabled})");

            return NoContent();
        }

        [HttpPost("test-connection")]
        public async Task<IActionResult> TestConnection([FromBody] LdapTestRequestDto request)
        {
            string host;
            int port;
            bool useSsl;
            string bindDn;
            string? bindPassword;

            if (request.UseStored)
            {
                var stored = await _service.GetAsync();
                host = stored.Host;
                port = stored.Port;
                useSsl = stored.UseSsl;
                bindDn = stored.BindDn;
                bindPassword = _service.DecryptPassword(stored.BindPasswordEncrypted);
            }
            else
            {
                host = request.Host ?? string.Empty;
                port = request.Port ?? 389;
                useSsl = request.UseSsl ?? false;
                bindDn = request.BindDn ?? string.Empty;
                bindPassword = request.BindPassword;
            }

            var result = new LdapTestResultDto();

            if (string.IsNullOrWhiteSpace(host))
            {
                result.Success = false;
                result.Message = "Host no configurado.";
                return Ok(result);
            }

            try
            {
                using var connection = new LdapConnection(new LdapDirectoryIdentifier(host, port));
                connection.SessionOptions.SecureSocketLayer = useSsl;
                connection.AuthType = AuthType.Basic;
                connection.Credential = new NetworkCredential(bindDn, bindPassword);
                connection.Timeout = TimeSpan.FromSeconds(10);
                connection.Bind();

                result.Success = true;
                result.Message = $"Conexión exitosa con {host}:{port}.";

                await _audit.LogAsync(AuditActions.LdapConfigTested, success: true,
                    description: $"Test LDAP OK ({host}:{port})");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";

                await _audit.LogAsync(AuditActions.LdapConfigTested, success: false,
                    description: $"Test LDAP fallido ({host}:{port}): {ex.Message}");
            }

            return Ok(result);
        }
    }
}
