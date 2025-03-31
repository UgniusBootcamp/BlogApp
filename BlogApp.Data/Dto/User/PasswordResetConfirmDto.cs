using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class PasswordResetConfirmDto
    {
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
