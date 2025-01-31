using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Auth.Data.Identity
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]

        [EmailAddress]
        public string Email { get; set; }
    }
}
