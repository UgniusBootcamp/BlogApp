using BlogApp.Business.Interfaces;
using BlogApp.Data.Dto.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AccountController(IAccountService accountService, IEmailService emailService, IMessageService messageService) : Controller
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

            var confirmationMessage = await messageService.CreateConfirmationMessageAsync(user, registerDto.ClientUri!);

            await emailService.SendEmailAsync(confirmationMessage);

            return RedirectToAction("Login");
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

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> PasswordReset(PasswordResetDto passwordResetDto)
        {
            var user = await accountService.GetUserByEmailAsync(passwordResetDto.Email);

            var resetMessage = await messageService.CreateResetMessageAsync(user, passwordResetDto.ClientUri);

            await emailService.SendEmailAsync(resetMessage);

            return RedirectToAction("Login");
        }

        [HttpPost]
        [Route("PasswordReset/Confirm")]
        public async Task<IActionResult> PasswordResetConfirm(PasswordResetConfirmDto passwordResetConfirm)
        {
            await accountService.ResetPasswordAsync(passwordResetConfirm);

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("Users/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await accountService.GetUserByIdAsync(userId);

            return View("Users",user);
        }

        [HttpPatch]
        [Route("Users/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string userId, UserUpdateDto userDto)
        {
            var user = await accountService.UpdateUserAsync(userId, userDto);

            return View("Users", user);
        }

    }
}
