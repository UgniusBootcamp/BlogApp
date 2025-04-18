using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.Comment
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public UserDetailDto User { get; set; } = null!;

    }
}
