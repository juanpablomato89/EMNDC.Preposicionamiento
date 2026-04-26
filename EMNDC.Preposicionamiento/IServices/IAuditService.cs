using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IAuditService
    {
        Task LogAsync(
            string action,
            bool success = true,
            string? userId = null,
            string? userEmail = null,
            string? entityType = null,
            string? entityId = null,
            string? description = null);

        Task<AuditLog> RecordAsync(AuditLog log);
    }
}
