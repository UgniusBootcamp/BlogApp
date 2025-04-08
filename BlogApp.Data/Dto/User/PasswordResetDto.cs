using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class PasswordResetDto
    {
        [Required(ErrorMessage = DisplayConstants.emailAddressIsRequired)]
        [EmailAddress(ErrorMessage = DisplayConstants.pleaseEnterAValidEmailAddress)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = DisplayConstants.emailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.pleaseConfirmYourEmailAddress)]
        [DataType(DataType.EmailAddress)]
        [Compare(DisplayConstants.email, ErrorMessage = DisplayConstants.emailAddressesDoNotMatch)]
        [Display(Name = DisplayConstants.confirmEmail)]
        public string ConfirmEmail { get; set; } = null!;
    }
}
