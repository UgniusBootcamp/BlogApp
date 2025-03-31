using BlogApp.Business.Interfaces;
using BlogApp.Data.Dto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AccountController(IAccountService accountService, IEmailService emailService) : Controller
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var principal = await accountService.LoginAsync(loginDto);

            if (principal != null) 
            {
                await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                });

                return RedirectToAction("Index", "Home");
            }

            return View(); 
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await accountService.RegisterAsync(registerDto);

            var confirmationMessage = await accountService.CreateConfirmationMessageAsync(user, registerDto.ClientUri!);

            await emailService.SendEmailAsync(confirmationMessage);

            return View("Login");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await accountService.ConfirmEmailAsync(email, token);

            return RedirectToAction("Login");
        }
    }
}
