namespace EMNDC.Preposicionamiento.Models
{
    public enum AlertEventType
    {
        FailedLoginThreshold = 1,
        NewAdminCreated = 2,
        RoleDeleted = 3,
        MultipleSessionsRevoked = 4,
        LdapConfigChanged = 5,
    }

    public class AlertRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AlertEventType EventType { get; set; }
        public int? Threshold { get; set; }
        public int? WindowMinutes { get; set; }
        public bool Enabled { get; set; } = true;
        public string NotifyEmails { get; set; } = string.Empty; // CSV
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class AlertNotification
    {
        public long Id { get; set; }
        public int AlertRuleId { get; set; }
        public AlertRule? AlertRule { get; set; }
        public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
        public string Payload { get; set; } = string.Empty;
        public bool Sent { get; set; }
        public DateTime? SentAt { get; set; }
        public string? Error { get; set; }
    }
}
