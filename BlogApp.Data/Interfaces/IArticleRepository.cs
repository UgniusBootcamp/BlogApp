using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Data.Interfaces
{
    public interface IArticleRepository
    {
        /// <summary>
        /// Method to get paginated list of articles
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id</param>
        /// <returns>paginated list of articles</returns>
        Task<PaginatedList<Article>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null);

        /// <summary>
        /// Method to get articles
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <param name="count">count</param>
        /// <returns>list of articles</returns>
        Task<IEnumerable<Article>> GetArticlesAsync(string? searchString, int count);

        /// <summary>
        /// Method to get article
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>article</returns>
        Task<Article?> GetArticleAsync(int id);

        /// <summary>
        /// Method to create article
        /// </summary>
        /// <param name="article">article to create</param>
        Task CreateArticleAsync(Article article);

        /// <summary>
        /// Method to update article
        /// </summary>
        /// <param name="article">article to update</param>
        Task UpdateArticleAsync(Article article);

        /// <summary>
        /// Method to delete article
        /// </summary>
        /// <param name="article">article to delete</param>
        Task DeleteArticleAsync(Article article);

        /// <summary>
        /// Method to get top rated articles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>top rated articles</returns>
        Task<IEnumerable<Article>> GetTopArticlesAsync(int count);

        /// <summary>
        /// Method to get new articles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>list of new articles</returns>
        Task<IEnumerable<Article>> LastArticlesAsync(int count);

        /// <summary>
        /// Method to get last commented articles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>list of last commented articles</returns>
        Task<IEnumerable<Article>> LastCommentedArticlesAsync(int count);
    }
}
