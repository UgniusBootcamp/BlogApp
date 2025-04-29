using BlogApp.Data.Dto.ArticleVote;
using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.Article
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; } = null!;
    }
}
