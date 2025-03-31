using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class PasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string ClientUri { get; set; } = null!;
    }
}
