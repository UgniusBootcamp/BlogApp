using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ArticleRepository(BlogAppDbContext context) : IArticleRepository
    {
        /// <summary>
        /// Method to create article
        /// </summary>
        /// <param name="article">article</param>
        public async Task CreateArticleAsync(Article article)
        {
            await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to delete article
        /// </summary>
        /// <param name="article">article</param>
        public async Task DeleteArticleAsync(Article article)
        {
            context.Articles.Remove(article);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get article
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>article</returns>
        public async Task<Article?> GetArticleAsync(int id)
        {
            return await context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Method to get paginated list of articles
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id</param>
        /// <returns>paginated list of articles</returns>
        public async Task<PaginatedList<Article>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null)
        {
            var query = context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(a => a.UserId == userId);

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.User)
                .ToListAsync();

            return new PaginatedList<Article>(items, pageIndex, totalPages);
        }

        /// <summary>
        /// Method to get articles
        /// </summary>
        /// <param name="searchString">seartch string</param>
        /// <param name="count">count of artciles</param>
        /// <returns>list of articles</returns>
        public async Task<IEnumerable<Article>> GetArticlesAsync(string? searchString, int count)
        {

            var query = context.Articles
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();

            if(!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();
                query = query.Where(a => a.Title.ToLower().Contains(search));
            }

            return await query
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Method to get top rated artciles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>top articles</returns>
        public async Task<IEnumerable<Article>> GetTopArticlesAsync(int count)
        {
            return await context.Articles
                .Include(a => a.User)
                .OrderByDescending(a =>
                    a.ArticleVotes.Count(v => v.VoteValue == true) -
                    a.ArticleVotes.Count(v => v.VoteValue == false))
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Method to get newest articles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>newest articles</returns>
        public async Task<IEnumerable<Article>> LastArticlesAsync(int count)
        {
            return await context.Articles
               .Include(a => a.User)
               .OrderByDescending(a => a.CreatedAt)
               .Take(count)
               .ToListAsync();
        }

        /// <summary>
        /// Method to get last commented articles
        /// </summary>
        /// <param name="count">count</param>
        /// <returns>last commented articles</returns>
        public async Task<IEnumerable<Article>> LastCommentedArticlesAsync(int count)
        {
            return await context.Articles
                .Include(a => a.User)
                .Include(a => a.Comments)
                .OrderByDescending(a => a.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => c.CreatedAt)
                    .FirstOrDefault())
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Method to update articles
        /// </summary>
        /// <param name="article">article to update</param>
        public async Task UpdateArticleAsync(Article article)
        {
            context.Update(article);
            await context.SaveChangesAsync();
        }
    }
}
