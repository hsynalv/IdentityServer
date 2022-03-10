using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Client1.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passowrd { get; set; }
    }
}
