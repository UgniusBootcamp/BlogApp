using BlogApp.Data.Entities;

namespace BlogApp.Data.Interfaces
{
    public interface IReportRepository
    {
        public Task CreateReportAsync(Report report);
        public Task<Report?> GetReportAsync(int commentId, string userId);
        public Task<int> GetCommentReportsCount(int commentId);
    }
}
