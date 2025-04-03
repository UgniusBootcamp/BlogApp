using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = DisplayConstants.pleaseEnterYourFirstName)]
        [StringLength(50, ErrorMessage = DisplayConstants.firstNameCannotExceed50Characters)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.pleaseEnterYourLastName)]
        [StringLength(50, ErrorMessage = DisplayConstants.lastNameCannotExceed50Characters)]
        public string Surname { get; set; } = null!;
    }
}
