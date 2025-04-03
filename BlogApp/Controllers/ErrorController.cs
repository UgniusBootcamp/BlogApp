using BlogApp.Data.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {

        [HttpGet(ControllerConstants.NotFound)]
        public IActionResult NotFoundError()
        {
            return View();
        }

        [HttpGet(ControllerConstants.ServerError)]
        public IActionResult ServerError()
        {
            return View();
        }

        [HttpGet(ControllerConstants.AccessDenied)]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
