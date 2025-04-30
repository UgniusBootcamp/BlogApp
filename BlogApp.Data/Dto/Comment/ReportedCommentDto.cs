using BlogApp.Data.Dto.Article;
using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.Comment
{
    public class ReportedCommentDto
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public ReportedArticleDto Article { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public UserDetailDto User { get; set; } = null!;
        public int ReportCount { get; set; }
    }
}
