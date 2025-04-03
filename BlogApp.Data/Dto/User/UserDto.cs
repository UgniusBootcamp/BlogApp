using BlogApp.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.User
{
    public class UserDto
    {
        [Display(Name = DisplayConstants.firstName)]
        public string Name { get; set; } = null!;

        [Display(Name = DisplayConstants.lastName)]
        public string Surname { get; set; } = null!;

        [Display(Name = DisplayConstants.lastName)]
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public IList<string> Roles { get; set; } = [];
        public bool EmailConfirmed { get; set; }
    }
}
