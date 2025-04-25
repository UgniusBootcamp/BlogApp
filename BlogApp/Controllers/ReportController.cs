using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Helpers.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class ReportController(IReportService reportService) : Controller
    {
        [HttpPost(ControllerConstants.CreateReport)]
        [Authorize(Roles = UserRoles.BlogUser)]
        public async Task<IActionResult> CreateReport(int articleId, int commentId)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await reportService.CreateReportAsync(userId, commentId);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ReportCreated;

            return RedirectToAction(ControllerConstants.Article, ControllerConstants.Article, new { id = articleId });
        }
    }
}
