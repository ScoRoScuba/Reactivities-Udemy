using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a=z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password needs to be more complex")]
        public string Password { get; set; }
        public string Username { get; set; }

    }
}
