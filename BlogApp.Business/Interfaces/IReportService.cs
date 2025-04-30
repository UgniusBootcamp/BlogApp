namespace BlogApp.Business.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Method to create report for article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="commentId">comment id</param>
        public Task CreateReportAsync(string userId, int commentId);
    }
}
