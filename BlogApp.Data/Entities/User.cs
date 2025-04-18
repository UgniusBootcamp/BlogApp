using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Surname { get; set; } = null!;

        public ICollection<RoleRequest> RoleRequests { get; set; } = [];
        public ICollection<Article> Articles { get; set; } = [];
        public ICollection<ArticleVote> ArticleVotes { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}
