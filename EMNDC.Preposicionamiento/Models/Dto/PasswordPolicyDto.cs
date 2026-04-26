namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class PasswordPolicyDto
    {
        public int MinLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int MaxFailedAttempts { get; set; }
        public int LockoutMinutes { get; set; }
        public int PasswordExpirationDays { get; set; }
        public int PreventReuseLastN { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? UpdatedByEmail { get; set; }
    }

    public class PasswordPolicyUpdateDto
    {
        public int MinLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int MaxFailedAttempts { get; set; }
        public int LockoutMinutes { get; set; }
        public int PasswordExpirationDays { get; set; }
        public int PreventReuseLastN { get; set; }
    }
}
