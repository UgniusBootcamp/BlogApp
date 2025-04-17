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
            if (!ModelState.IsValid)
                return PartialView(ControllerConstants._CommentCreate, createCommentDto);

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
            if (!ModelState.IsValid)
                return PartialView(ControllerConstants._CommentEdit, editCommentDto);

            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await commentService.EditCommentAsync(userId, editCommentDto);
            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.CommentEdited;

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

    }
}
