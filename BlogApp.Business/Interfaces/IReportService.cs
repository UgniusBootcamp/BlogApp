namespace BlogApp.Business.Interfaces
{
    public interface IReportService
    {
        public Task CreateReportAsync(string userId, int commentId);
    }
}
