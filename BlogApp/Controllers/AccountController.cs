using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult Login()
        {
            return View();
        }
    }
}
