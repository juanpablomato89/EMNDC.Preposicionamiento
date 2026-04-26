using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Http;

namespace EMNDC.Preposicionamiento.Services
{
    public class AuditService : IAuditService
    {
        private readonly PreposicionamientoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public AuditService(
            PreposicionamientoDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        public async Task LogAsync(
            string action,
            bool success = true,
            string? userId = null,
            string? userEmail = null,
            string? entityType = null,
            string? entityId = null,
            string? description = null)
        {
            var http = _httpContextAccessor.HttpContext;
            string? ip = http?.Connection?.RemoteIpAddress?.ToString();
            string? userAgent = http?.Request?.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(userId) && http?.User?.Identity?.IsAuthenticated == true)
            {
                try { userId = http.User.GetUserIdFromToken(); } catch { /* ignore */ }
                try { userEmail ??= http.User.GetUserEmailFromToken(); } catch { /* ignore */ }
            }

            var log = new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                UserEmail = userEmail,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Description = description,
                IpAddress = ip,
                UserAgent = userAgent,
                Success = success
            };

            await RecordAsync(log);
        }

        public async Task<AuditLog> RecordAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            // Disparar evaluador de alertas tras persistir el log.
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var evaluator = scope.ServiceProvider.GetService<IAlertEvaluator>();
                if (evaluator != null)
                {
                    await evaluator.EvaluateAsync(log);
                }
            }
            catch
            {
                // No interrumpir el flujo de auditoría por errores de alertas.
            }

            return log;
        }
    }
}
