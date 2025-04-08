using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = DisplayConstants.pleaseEnterYourFirstName)]
        [StringLength(50, ErrorMessage = DisplayConstants.firstNameCannotExceed50Characters)]
        [Display(Name = DisplayConstants.firstName)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.pleaseEnterYourLastName)]
        [StringLength(50, ErrorMessage = DisplayConstants.lastNameCannotExceed50Characters)]
        [Display(Name = DisplayConstants.lastName)]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.pleaseConfirmYourEmailAddress)]
        [EmailAddress(ErrorMessage = DisplayConstants.pleaseEnterAValidEmailAddress)]
        [Display(Name = DisplayConstants.emailAddress)]
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
        [Compare(DisplayConstants.password, ErrorMessage = DisplayConstants.passwordsDoNotMatch)]
        [Display(Name = DisplayConstants.confirmPassword)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
