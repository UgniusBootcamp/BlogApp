using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class CommentRepository(BlogAppDbContext context) : ICommentRepository
    {
        public async Task CreateCommentAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        public async Task EditCommentAsync(Comment comment)
        {
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<PaginatedList<Comment>> GetPaginatedCommentsAsync(int articleId, int pageIndex, int pageSize)
        {
            var query = context.Comments
                .Where(c => c.ArticleId == articleId)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.User)
                .ToListAsync();

            return new PaginatedList<Comment>(items, pageIndex, totalPages);
        }
    }
}
