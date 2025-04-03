using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = DisplayConstants.usernameOrEmailIsRequired)]
        [StringLength(255, ErrorMessage = DisplayConstants.NameCannotExceed255Characters)]
        [Display(Name = DisplayConstants.usernameOrEmail)]
        public string Credentials { get; set; } = null!;

        [Required(ErrorMessage = DisplayConstants.passwordIsRequired)]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
