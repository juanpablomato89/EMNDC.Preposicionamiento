using System.ComponentModel.DataAnnotations;

namespace EMNDC.Preposicionamiento.Models.Requests
{
    public class LoginRequests
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}