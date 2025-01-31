using System.ComponentModel.DataAnnotations;

namespace Auth.DTOs
{
    public class RegisterDto
    {
       
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
       
        public string Password { get; set; }
    }
}
