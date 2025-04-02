using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class UserDto
    {
        [Display(Name = "First name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Last name")]
        public string Surname { get; set; } = null!;

        [Display(Name = "Email address")]
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public IList<string> Roles { get; set; } = [];
        public bool EmailConfirmed { get; set; }
    }
}
