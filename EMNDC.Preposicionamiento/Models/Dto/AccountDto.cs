namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class AccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? OrganismoId { get; set; }
        public string? Organismo { get; set; }
        public string? Role { get; set; }
        public bool IsUserDomain { get; set; }
        public DateTime Creado { get; set; }
    }

    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ChangeEmailDto
    {
        public string NewEmail { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
