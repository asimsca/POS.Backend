using System.ComponentModel.DataAnnotations;

namespace POS.Backend.DTO.Request.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
