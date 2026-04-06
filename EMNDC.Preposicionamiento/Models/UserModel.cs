using Microsoft.AspNetCore.Identity;

namespace EMNDC.Preposicionamiento.Models
{
    public class UserModel : IdentityUser
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? Code { get; set; }
        public string? ProviderId { get; set; }
        public DateTime DateGeneratedCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifieddAt { get; set; }
        public bool IsUserDomain { get; set; } = false;
    }
}
