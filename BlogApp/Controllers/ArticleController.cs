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
        IMapper mapper
        ) : Controller
    {
        [HttpGet(ControllerConstants.Articles)]
        public async Task<IActionResult> Articles(int pageIndex = 1, int pageSize = 10)
        {
            var articles = await articleService.GetArticlesAsync(pageIndex, pageSize);
            return View(articles);
        }

        [HttpGet(ControllerConstants.Article)]
        public async Task<IActionResult> Article(int id)
        {
            var article = await articleService.GetArticleAsync(id);
            return View(article);
        }

        [HttpGet(ControllerConstants.MyArticles)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> MyArticles(int pageIndex = 1, int pageSize = 10)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            var myArticles = await articleService.GetArticlesAsync(pageIndex, pageSize, userId);

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
    }
}
