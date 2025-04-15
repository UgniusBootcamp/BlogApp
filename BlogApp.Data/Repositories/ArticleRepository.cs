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

        public async Task UpdateArticleAsync(Article article)
        {
            context.Update(article);
            await context.SaveChangesAsync();
        }
    }
}
