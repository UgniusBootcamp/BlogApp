using BlogApp.Data.Constants;
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
        [Required(ErrorMessage = DisplayConstants.passwordIsRequired)]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = DisplayConstants.passwordMustBeBetween8And100Characters, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,100}$",
            ErrorMessage = DisplayConstants.passwordMustContain)]
        [Display(Name = DisplayConstants.password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.pleaseConfirmYourPassword)]
        [DataType(DataType.Password)]
        [Compare(DisplayConstants.password, ErrorMessage = DisplayConstants.pleaseConfirmYourPassword)]
        [Display(Name = DisplayConstants.passwordsDoNotMatch)]
        public string ConfirmPassword { get; set; } = null!;

    }
}
