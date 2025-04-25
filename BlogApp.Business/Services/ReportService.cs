using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Business.Services
{
    public class ReportService(IReportRepository reportRepository) : IReportService
    {
        public async Task CreateReportAsync(string userId, int commentId)
        {
            var exists = await reportRepository.GetReportAsync(commentId, userId);

            if (exists != null)
                throw new BusinessRuleValidationException(ServiceConstants.ReportCanBeCreatedOnceByUser);

            var report = new Report
            {
                CommentId = commentId,
                UserId = userId,
            };

            await reportRepository.CreateReportAsync(report);
        }
    }
}
