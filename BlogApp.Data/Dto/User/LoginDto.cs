using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class LoginDto
    {
        [Required]
        [StringLength(255)]
        public string Credentials { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
