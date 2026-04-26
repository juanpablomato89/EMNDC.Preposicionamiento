namespace EMNDC.Preposicionamiento.Models
{
    public class PasswordPolicy
    {
        public int Id { get; set; }
        public int MinLength { get; set; } = 8;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = false;
        public int MaxFailedAttempts { get; set; } = 5;
        public int LockoutMinutes { get; set; } = 15;
        public int PasswordExpirationDays { get; set; } = 0; // 0 = nunca
        public int PreventReuseLastN { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedByUserId { get; set; }
    }
}
