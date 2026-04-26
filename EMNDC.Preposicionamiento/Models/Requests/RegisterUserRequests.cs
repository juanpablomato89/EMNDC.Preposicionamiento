using System.ComponentModel.DataAnnotations;

namespace EMNDC.Preposicionamiento.Models.Requests
{
    public class RegisterUserRequests
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string ConfirmationPassword { get; set; }

        public int? OrganismoId { get; set; }
    }
}
