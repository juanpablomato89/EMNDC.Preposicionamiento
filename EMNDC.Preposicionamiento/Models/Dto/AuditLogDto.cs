namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class AuditLogDto
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool Success { get; set; }
    }
}
