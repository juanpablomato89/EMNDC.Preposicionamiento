namespace EMNDC.Preposicionamiento.Models
{
    public class TokenModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; }
        public UserModel User { get; set; }
    }
}
