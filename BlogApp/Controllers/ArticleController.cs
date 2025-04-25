using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.Article;
using BlogApp.Data.Helpers.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class ArticleController(
        IArticleService articleService,
        IArticleVoteService articleVoteService,
        ICommentService commentService,
        IMapper mapper
        ) : Controller
    {
        [HttpGet(ControllerConstants.Articles)]
        public async Task<IActionResult> Articles(int pageIndex = 1, int pageSize = 10)
        {
            var articles = await articleService.GetArticlesAsync(pageIndex, pageSize);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            foreach (var article in articles.Items)
            {
                article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, userId);
            }

            return View(articles);
        }

        [HttpGet(ControllerConstants.Article)]
        public async Task<IActionResult> Article(int id, int pageIndex = 1, int pageSize = 10)
        {
            var article = await articleService.GetArticleAsync(id);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, userId);
            article.PaginatedComments = await commentService.GetArticleCommentsAsync(id, pageIndex, pageSize, userId);

            return View(article);
        }

        [HttpGet(ControllerConstants.MyArticles)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> MyArticles(int pageIndex = 1, int pageSize = 10)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            var myArticles = await articleService.GetArticlesAsync(pageIndex, pageSize, userId);

            foreach (var article in myArticles.Items)
            {
                article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, userId);
            }

            return View(myArticles);
        }

        [HttpGet(ControllerConstants.CreateArticle)]
        [Authorize(Roles = UserRoles.Author)]
        public IActionResult CreateArticle()
        {
            return View(new ArticleCreateDto());
        }

        [HttpPost(ControllerConstants.CreateArticle)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleCreateDto articleCreateDto)
        {
            if (!ModelState.IsValid)
                return View(articleCreateDto);

            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            await articleService.CreateArticleAsync(userId, articleCreateDto);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ArticleHasBeenCreated;
            return RedirectToAction(ControllerConstants.MyArticles);
        }
        
        [HttpGet(ControllerConstants.UpdateArticle)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> UpdateArticle(int id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            var article = await articleService.GetArticleAsync(id);

            if(article.User.Id != userId)
                return RedirectToAction(ControllerConstants.AccessDenied, ControllerConstants.Error);

            var articleUpdateDto = mapper.Map<ArticleUpdateDto>(article);

            return View(articleUpdateDto);
        }

        [HttpPost(ControllerConstants.UpdateArticle)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleUpdateDto articleUpdateDto)
        {
            if (!ModelState.IsValid)
                return View(articleUpdateDto);

            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            await articleService.UpdateArticleAsync(userId, articleUpdateDto);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ArticleHasBeenUpdated;
            return RedirectToAction(ControllerConstants.MyArticles);
        }

        [HttpPost(ControllerConstants.DeleteArticle)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await articleService.DeleteArticleAsync(userId, id);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ArticleHasBeenDeleted;

            return RedirectToAction(ControllerConstants.MyArticles);
        }

        [HttpGet(ControllerConstants.ArticleSearch)]
        public async Task<IActionResult> ArticleSearch(string? searchString, int count = 5)
        {
            if(string.IsNullOrEmpty(searchString))
                return PartialView(ControllerConstants._ArticleSearch, new List<ArticleTagDto>());

            var articles = await articleService.GetArticleTagsAsync(searchString, count);

            return PartialView(ControllerConstants._ArticleSearch, articles);
        }
    }
}
