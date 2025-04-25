using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ReportRepository(BlogAppDbContext context) : IReportRepository
    {
        public async Task CreateReportAsync(Report report)
        {
            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();
        }

        public async Task<int> GetCommentReportsCount(int commentId)
        {
            return await context.Reports
                .CountAsync(r => r.CommentId == commentId);
        }

        public async Task<Report?> GetReportAsync(int commentId, string userId)
        {
            return await context.Reports
                .FirstOrDefaultAsync(r => r.CommentId == commentId && r.UserId == userId);
        }
    }
}
