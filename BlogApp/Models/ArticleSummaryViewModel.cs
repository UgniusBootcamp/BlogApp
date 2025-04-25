using BlogApp.Data.Dto.Article;

namespace BlogApp.Models
{
    public class ArticleSummaryViewModel
    {
        public IEnumerable<ArticleListDto> LastArticles = [];
        public IEnumerable<ArticleListDto> TopVotedArticles = [];
        public IEnumerable<ArticleWithCommentDto> LastCommentedArticles = [];
    }
}
