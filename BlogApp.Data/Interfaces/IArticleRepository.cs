using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Data.Interfaces
{
    public interface IArticleRepository
    {
        Task<PaginatedList<Article>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null);
        Task<Article?> GetArticleAsync(int id);
        Task CreateArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(Article article);
    }
}
