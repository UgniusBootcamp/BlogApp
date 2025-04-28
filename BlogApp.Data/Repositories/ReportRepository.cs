using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ReportRepository(BlogAppDbContext context) : IReportRepository
    {
        /// <summary>
        /// Method to create a report for a comment
        /// </summary>
        /// <param name="report">report to create</param>
        public async Task CreateReportAsync(Report report)
        {
            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get comment reports count
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>reports count on comment</returns>
        public async Task<int> GetCommentReportsCount(int commentId)
        {
            return await context.Reports
                .CountAsync(r => r.CommentId == commentId);
        }

        /// <summary>
        /// method to get report by comment id and user id
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<Report?> GetReportAsync(int commentId, string userId)
        {
            return await context.Reports
                .FirstOrDefaultAsync(r => r.CommentId == commentId && r.UserId == userId);
        }
    }
}
