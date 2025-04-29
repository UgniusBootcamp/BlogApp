using BlogApp.Business.Interfaces;
using BlogApp.Business.Services;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.Article;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Roles;
using BlogApp.Data.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogApp.Api.Controllers
{
    [ApiController]
    [Route(ControllerConstants.api + "[controller]")]
    public class BlogController(
        IArticleService articleService,
        IArticleVoteService articleVoteService,
        ICommentService commentService
        ) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Blogs(int pageIndex = 1, int pageSize = 10)
        {
            var articles = await articleService.GetArticlesAsync(pageIndex, pageSize);
            foreach (var article in articles.Items)
            {
                article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, null);
            }

            return Ok(ApiResponse.OkResponse("", articles));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Blog(int id, int pageIndex = 1, int pageSize = 10)
        {
            var article = await articleService.GetArticleAsync(id);

            article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, null);
            article.PaginatedComments = await commentService.GetArticleCommentsAsync(id, pageIndex, pageSize, null);

            return Ok(ApiResponse.OkResponse("", article));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> CreateBlog([FromBody] ArticleCreateDto articleCreateDto)
        {
            var userId = HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var created = await articleService.CreateArticleAsync(userId, articleCreateDto);

            return Created("", ApiResponse.CreatedResponse(ControllerConstants.ArticleCreated, created));
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> UpdateBlog([FromBody] ArticleUpdateDto articleUpdateDto)
        {
            var userId = HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var updated = await articleService.UpdateArticleAsync(userId, articleUpdateDto);
            return Ok(ApiResponse.OkResponse(ControllerConstants.ArticleUpdated,updated));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var userId = HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            await articleService.DeleteArticleAsync(userId, id);

            return Ok(ApiResponse.OkResponse(ControllerConstants.ArticleDeleted)); ;
        }

    }
}
