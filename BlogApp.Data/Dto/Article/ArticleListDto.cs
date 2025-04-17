using BlogApp.Data.Dto.ArticleVote;
using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.Article
{
    public class ArticleListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public UserDetailDto User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ArticleVoteReadDto Vote { get; set; } = null!;
    }
}
