using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.ArticleVote;
using BlogApp.Data.Helpers.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class ArticleVoteController(
        IArticleVoteService articleVoteService
        ) : Controller
    {
        [HttpPost(ControllerConstants.ArticleVote)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Voter)]
        public async Task<IActionResult> ArticleVote(ArticleVoteCreateDto artcileVoteCreateDto)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            await articleVoteService.VoteAsync(userId, artcileVoteCreateDto);

            var articleVoteReadDto = await articleVoteService.GetArticleVotesAsync(artcileVoteCreateDto.ArticleId, userId);

            return PartialView(ControllerConstants._ArticleVote, articleVoteReadDto);
        }
    }
}
