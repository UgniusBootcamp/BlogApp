using BlogApp.Data.Dto.Article;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface IArticleService
    {
        /// <summary>
        /// Method to get paginated list of articles
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id if filtering is needed</param>
        /// <returns>paginated list of articles</returns>
        public Task<PaginatedList<ArticleListDto>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null);

        public Task<IEnumerable<ArticleTagDto>> GetArticleTagsAsync(string? searchString, int count);

        /// <summary>
        /// Method to get article by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>article</returns>
        public Task<ArticleDetailDto> GetArticleAsync(int id);

        /// <summary>
        /// Method to create article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="articleCreateDto">article create dto</param>
        public Task CreateArticleAsync(string userId, ArticleCreateDto articleCreateDto);

        /// <summary>
        /// Method to update article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="articleUpdateDto">article update dto</param>
        public Task UpdateArticleAsync(string userId, ArticleUpdateDto articleUpdateDto);

        /// <summary>
        /// Method to delete article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="id">article id</param>
        public Task DeleteArticleAsync(string userId, int id);
        public Task<IEnumerable<ArticleListDto>> GetTopArticlesAsync(int count);
        public Task<IEnumerable<ArticleListDto>> LastArticlesAsync(int count);
        public Task<IEnumerable<ArticleWithCommentDto>> LastCommentedArticlesAsync(int count);
    }
}
