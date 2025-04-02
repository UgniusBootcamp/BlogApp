using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class PasswordResetDto
    {
        [Required(ErrorMessage ="Password is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your email adress")]
        [DataType(DataType.EmailAddress)]
        [Compare("Email", ErrorMessage = "Email addresses do not match")]
        [Display(Name = "Confirm Email")]
        public string ConfirmEmail { get; set; } = null!;
    }
}
