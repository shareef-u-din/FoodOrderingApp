using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.Requests
{
    public class UserRegistrationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        public string Phone { get; set; }
    }
}
