using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class CommentRepository(BlogAppDbContext context) : ICommentRepository
    {
        /// <summary>
        /// Method to create comment
        /// </summary>
        /// <param name="comment">comment to create</param>
        public async Task CreateCommentAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to delete comment
        /// </summary>
        /// <param name="comment">comment to delete</param>
        public async Task DeleteCommentAsync(Comment comment)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to edit comment
        /// </summary>
        /// <param name="comment">comment to edit</param>
        public async Task EditCommentAsync(Comment comment)
        {
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get comment by id
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>comment</returns>
        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<Comment?> GetLastArticleCommentByIdAsync(int articleId)
        {
            return await context.Comments
                .Where(c => c.ArticleId == articleId)
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.User)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Method to get paginated comments
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated article comments</returns>
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

        public async Task<PaginatedList<Comment>> GetPaginatedReportedCommentsAsync(int pageIndex, int pageSize)
        {
            var query = context.Comments.Where(c => c.Reports.Count > 0)
                            .OrderByDescending(c => c.CreatedAt)
                            .AsQueryable();

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.User)
                .Include(a => a.Article)
                .ToListAsync();

            return new PaginatedList<Comment>(items, pageIndex, totalPages);
        }
    }
}
