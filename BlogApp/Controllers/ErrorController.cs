using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {

        [HttpGet("NotFound")]
        public IActionResult NotFoundError()
        {
            return View();
        }

        [HttpGet("ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
