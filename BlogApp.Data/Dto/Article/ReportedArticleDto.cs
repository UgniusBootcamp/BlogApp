namespace BlogApp.Data.Dto.Article
{
    public class ReportedArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
