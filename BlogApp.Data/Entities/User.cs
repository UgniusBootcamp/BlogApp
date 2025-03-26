using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
    }
}
