using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Data.Entities
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }


        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public ICollection<ArticleVote> ArticleVotes { get; set; } = [];
    }
}
