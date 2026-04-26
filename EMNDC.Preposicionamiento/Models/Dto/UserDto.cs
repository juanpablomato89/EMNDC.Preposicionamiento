namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? OrganismoId { get; set; }
        public string? Organismo { get; set; }
        public string? Role { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsUserDomain { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Modificado { get; set; }
    }

    public class UserCreateDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? OrganismoId { get; set; }
        public string Role { get; set; } = "User";
    }

    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? OrganismoId { get; set; }
        public string? Role { get; set; }
    }

    public class ResetPasswordAdminDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }

    public class LockUserDto
    {
        public bool Lock { get; set; }
    }
}
