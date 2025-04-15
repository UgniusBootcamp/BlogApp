using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class EmailConfirmationResendDto
    {
        [Required(ErrorMessage = DisplayConstants.pleaseConfirmYourEmailAddress)]
        [EmailAddress(ErrorMessage = DisplayConstants.pleaseEnterAValidEmailAddress)]
        [Display(Name = DisplayConstants.emailAddress)]
        public string Email { get; set; } = null!;
    }
}
