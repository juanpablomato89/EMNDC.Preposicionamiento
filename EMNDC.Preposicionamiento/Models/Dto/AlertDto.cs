using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class AlertRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AlertEventType EventType { get; set; }
        public string EventTypeName { get; set; } = string.Empty;
        public int? Threshold { get; set; }
        public int? WindowMinutes { get; set; }
        public bool Enabled { get; set; }
        public List<string> NotifyEmails { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AlertRuleCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AlertEventType EventType { get; set; }
        public int? Threshold { get; set; }
        public int? WindowMinutes { get; set; }
        public bool Enabled { get; set; } = true;
        public List<string> NotifyEmails { get; set; } = new();
    }

    public class AlertRuleUpdateDto : AlertRuleCreateDto { }

    public class AlertNotificationDto
    {
        public long Id { get; set; }
        public int AlertRuleId { get; set; }
        public string AlertRuleName { get; set; } = string.Empty;
        public DateTime TriggeredAt { get; set; }
        public string Payload { get; set; } = string.Empty;
        public bool Sent { get; set; }
        public DateTime? SentAt { get; set; }
        public string? Error { get; set; }
    }
}
