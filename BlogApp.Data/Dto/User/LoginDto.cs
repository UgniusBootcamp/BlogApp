using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Username or Email is required")]
        [StringLength(255, ErrorMessage ="Name cannot exceed 255 characters")]
        [Display(Name = "Username or Email")]
        public string Credentials { get; set; } = null!;

        [Required(ErrorMessage ="Password is required")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
