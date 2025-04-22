using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ArticleRepository(BlogAppDbContext context) : IArticleRepository
    {
        public async Task CreateArticleAsync(Article article)
        {
            await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(Article article)
        {
            context.Articles.Remove(article);
            await context.SaveChangesAsync();
        }

        public async Task<Article?> GetArticleAsync(int id)
        {
            return await context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

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

        public async Task<IEnumerable<Article>> GetArticlesAsync(string searchString, int count)
        {
            return await context.Articles
                .Include(a => a.User)
                .Where(a => a.Title.Contains(searchString))
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

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

        public async Task<IEnumerable<Article>> LastArticlesAsync(int count)
        {
            return await context.Articles
               .Include(a => a.User)
               .OrderByDescending(a => a.CreatedAt)
               .Take(count)
               .ToListAsync();
        }

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

        public async Task UpdateArticleAsync(Article article)
        {
            context.Update(article);
            await context.SaveChangesAsync();
        }
    }
}
