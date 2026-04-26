using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Dto;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class PasswordPolicyController : ControllerBase
    {
        private readonly IPasswordPolicyService _service;
        private readonly PreposicionamientoDbContext _context;

        public PasswordPolicyController(IPasswordPolicyService service, PreposicionamientoDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var p = await _service.GetAsync();
            string? email = null;
            if (!string.IsNullOrEmpty(p.UpdatedByUserId))
            {
                email = await _context.Users
                    .Where(u => u.Id == p.UpdatedByUserId)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();
            }
            return Ok(new PasswordPolicyDto
            {
                MinLength = p.MinLength,
                RequireUppercase = p.RequireUppercase,
                RequireLowercase = p.RequireLowercase,
                RequireDigit = p.RequireDigit,
                RequireNonAlphanumeric = p.RequireNonAlphanumeric,
                MaxFailedAttempts = p.MaxFailedAttempts,
                LockoutMinutes = p.LockoutMinutes,
                PasswordExpirationDays = p.PasswordExpirationDays,
                PreventReuseLastN = p.PreventReuseLastN,
                UpdatedAt = p.UpdatedAt,
                UpdatedByUserId = p.UpdatedByUserId,
                UpdatedByEmail = email
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PasswordPolicyUpdateDto dto)
        {
            if (dto.MinLength < 4 || dto.MinLength > 64)
                return BadRequest("MinLength debe estar entre 4 y 64.");
            if (dto.MaxFailedAttempts < 0)
                return BadRequest("MaxFailedAttempts debe ser >= 0.");
            if (dto.LockoutMinutes < 0)
                return BadRequest("LockoutMinutes debe ser >= 0.");

            var userId = User.GetUserIdFromToken();

            await _service.UpdateAsync(new PasswordPolicy
            {
                MinLength = dto.MinLength,
                RequireUppercase = dto.RequireUppercase,
                RequireLowercase = dto.RequireLowercase,
                RequireDigit = dto.RequireDigit,
                RequireNonAlphanumeric = dto.RequireNonAlphanumeric,
                MaxFailedAttempts = dto.MaxFailedAttempts,
                LockoutMinutes = dto.LockoutMinutes,
                PasswordExpirationDays = dto.PasswordExpirationDays,
                PreventReuseLastN = dto.PreventReuseLastN,
            }, userId);

            return NoContent();
        }
    }
}
