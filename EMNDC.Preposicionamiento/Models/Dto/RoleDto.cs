namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int UsersCount { get; set; }
        public bool IsSystem { get; set; }
    }

    public class RoleCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class RoleUpdateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
