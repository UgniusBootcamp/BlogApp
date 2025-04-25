using System.Diagnostics;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class HomeController(
        IArticleService articleService
        ) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var viewModel = new ArticleSummaryViewModel
            {
                LastArticles = await articleService.LastArticlesAsync(ControllerConstants.LastArticlesCount),
                TopVotedArticles = await articleService.GetTopArticlesAsync(ControllerConstants.TopArticlesCount),
                LastCommentedArticles = await articleService.LastCommentedArticlesAsync(ControllerConstants.LastCommentedArticlesCount)
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
