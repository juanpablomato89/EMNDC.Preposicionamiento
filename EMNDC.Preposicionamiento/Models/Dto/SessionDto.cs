namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsExpired { get; set; }
        public bool IsActive { get; set; }
    }
}
