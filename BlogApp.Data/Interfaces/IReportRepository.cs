using BlogApp.Data.Entities;

namespace BlogApp.Data.Interfaces
{
    public interface IReportRepository
    {
        /// <summary>
        /// Method to create a report for a comment
        /// </summary>
        /// <param name="report">report to create</param>
        public Task CreateReportAsync(Report report);

        /// <summary>
        /// Method to get report by comment id and user id
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="userId">user id</param>
        /// <returns>report</returns>
        public Task<Report?> GetReportAsync(int commentId, string userId);

        /// <summary>
        /// Method to get comment reports count
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>comments reports count</returns>
        public Task<int> GetCommentReportsCount(int commentId);
    }
}
