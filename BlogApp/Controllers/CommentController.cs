using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.Comment;
using BlogApp.Data.Helpers.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class CommentController(ICommentService commentService) : Controller
    {
        [HttpPost(ControllerConstants.CreateComment)]
        [Authorize(Roles = UserRoles.Commentator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CommentCreateDto createCommentDto)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await commentService.CreateCommentAsync(userId, createCommentDto);
            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.CommentCreated;

            return RedirectToAction(ControllerConstants.Article, ControllerConstants.Article, new { id = createCommentDto.ArticleId });
        }

        [HttpPost(ControllerConstants.EditComment)]
        [Authorize(Roles = UserRoles.Commentator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(CommentEditDto editCommentDto)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await commentService.EditCommentAsync(userId, editCommentDto);

            var comment = await commentService.GetCommentByIdAsync(editCommentDto.CommentId);

            return PartialView(ControllerConstants._Comment, comment);
        }

        [HttpPost(ControllerConstants.DeleteComment)]
        [Authorize(Roles = UserRoles.Commentator)]
        public async Task<IActionResult> DeleteComment(int articleId, int commentId)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await commentService.DeleteCommentAsync(userId, commentId);
            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.CommentDeleted;

            return RedirectToAction(ControllerConstants.Article, ControllerConstants.Article, new { id = articleId });
        }

        [HttpGet(ControllerConstants.ReportedComments)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ReportedComments(int pageIndex = 1, int pageSize = 10)
        {
            var reportedComments = await commentService.GetPaginatedReportedCommentsAsync(pageIndex, pageSize);

            return View(reportedComments);
        }

        [HttpPost(ControllerConstants.DeleteCommentAdmin)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteCommentAdmin(int articleId, int commentId)
        {
            await commentService.DeleteCommentAsync(commentId);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.CommentDeleted;

            return RedirectToAction(ControllerConstants.ReportedComments);
        }
    }
}
