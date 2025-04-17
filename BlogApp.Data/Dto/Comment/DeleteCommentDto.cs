namespace BlogApp.Data.Dto.Comment
{
    public class DeleteCommentDto
    {
        public int ArticleId { get; set; }
        public int CommentId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
