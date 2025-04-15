using BlogApp.Data.Dto.Article;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface IArticleService
    {
        public Task<PaginatedList<ArticleListDto>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null);
        public Task<ArticleDetailDto> GetArticleAsync(int id);
        public Task CreateArticleAsync(string userId, ArticleCreateDto articleCreateDto);
        public Task UpdateArticleAsync(string userId, ArticleUpdateDto articleUpdateDto);
        public Task DeleteArticleAsync(string userId, int id);
    }
}
