using BlogApp.Business.Interfaces;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Helpers.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class AccountController(IAccountService accountService, IEmailService emailService, IMessageService messageService) : Controller
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            try 
            { 
                await accountService.LoginAsync(loginDto); 
            }
            catch (UnauthorizedException ex)
            {
                ModelState.AddModelError("Password", ex.Message);
                return View(loginDto);
            }
            TempData["SnackbarMessage"] = "Log in Successful!";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if(User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new LoginDto());
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await accountService.LogOutAsync();

            TempData["SnackbarMessage"] = "Log out Successful!";

            return RedirectToAction("Login");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
                return View(registerDto);

            try
            {
                var user = await accountService.RegisterAsync(registerDto);

                if(user != null)
                {
                    var route = Url.Action("EmailConfirmation", "Account", null, Request.Scheme);

                    var confirmationMessage = await messageService.CreateConfirmationMessageAsync(user, route!);

                    await emailService.SendEmailAsync(confirmationMessage);

                    TempData["SnackbarMessage"] = "Confirmation Email has been sent. Chech Your Inbox.";

                    return RedirectToAction("Login");
                }
            }
            catch (UnauthorizedException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(registerDto);
            }

            return View(registerDto);
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterDto());
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await accountService.ConfirmEmailAsync(email, token);

            return View();
        }

        [HttpGet]
        [Route("PasswordReset")]
        public IActionResult PasswordReset()
        {
            return View(new PasswordResetDto());
        }

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> PasswordReset(PasswordResetDto passwordResetDto)
        {
            if (!ModelState.IsValid)
                return View(passwordResetDto);
            try
            {
                var user = await accountService.GetUserByEmailAsync(passwordResetDto.Email);

                var route = Url.Action("PasswordResetConfirm", "Account", null, Request.Scheme);

                var resetMessage = await messageService.CreateResetMessageAsync(user, route!);

                await emailService.SendEmailAsync(resetMessage);

            }
            catch (NotFoundException) { }
                

            TempData["SnackbarMessage"] = "Password Reset has been sent! Check Your Inbox";

            return View(passwordResetDto);
        }

        [HttpGet]
        [Route("PasswordReset/Confirm")]
        public IActionResult PasswordResetConfirm([FromQuery] string email, [FromQuery] string token)
        {
            return View(new PasswordResetConfirmDto { Email = email, Token = token });
        }

        [HttpPost]
        [Route("PasswordReset/Confirm")]
        public async Task<IActionResult> PasswordResetConfirm(PasswordResetConfirmDto passwordResetConfirm)
        {
            if (!ModelState.IsValid)
                return View(passwordResetConfirm);

            await accountService.ResetPasswordAsync(passwordResetConfirm);

            TempData["SnackbarMessage"] = "Password has been reset!";

            return RedirectToAction("Index", "Home");

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
