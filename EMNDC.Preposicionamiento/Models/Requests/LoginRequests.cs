using System.ComponentModel.DataAnnotations;

namespace EMNDC.Preposicionamiento.Models.Requests
{
    public class LoginRequests
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}