using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Helpers.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class AccountController(IAccountService accountService, IEmailService emailService, IMessageService messageService) : Controller
    {
        [HttpPost(ControllerConstants.Login)]
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
                ModelState.AddModelError(ControllerConstants.Password, ex.Message);
                return View(loginDto);
            }
            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.LogInSuccessful;

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }

        [HttpGet(ControllerConstants.Login)]
        public IActionResult Login()
        {
            if(User.Identity?.IsAuthenticated == true)
                return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

            return View(new LoginDto());
        }

        [HttpPost(ControllerConstants.Logout)]
        public async Task<IActionResult> Logout()
        {
            await accountService.LogOutAsync();

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.LogOutSuccessful;

            return RedirectToAction(ControllerConstants.Login);
        }

        [HttpPost(ControllerConstants.Register)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
                return View(registerDto);

            try
            {
                var user = await accountService.RegisterAsync(registerDto);

                if(user != null)
                {
                    var route = Url.Action(ControllerConstants.EmailConfirmation, ControllerConstants.Account, null, Request.Scheme);

                    var confirmationMessage = await messageService.CreateConfirmationMessageAsync(user, route!);

                    await emailService.SendEmailAsync(confirmationMessage);

                    TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ConfirmationEmailSent;

                    return RedirectToAction(ControllerConstants.Login);
                }
            }
            catch (UnauthorizedException ex)
            {
                ModelState.AddModelError(ControllerConstants.Email, ex.Message);
                return View(registerDto);
            }

            return View(registerDto);
        }

        [HttpGet(ControllerConstants.Register)]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

            return View(new RegisterDto());
        }

        [HttpGet(ControllerConstants.EmailConfirmation)]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await accountService.ConfirmEmailAsync(email, token);

            return View();
        }

        [HttpGet]
        [Route(ControllerConstants.PasswordReset)]
        public IActionResult PasswordReset()
        {
            return View(new PasswordResetDto());
        }

        [HttpPost]
        [Route(ControllerConstants.PasswordReset)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(PasswordResetDto passwordResetDto)
        {
            if (!ModelState.IsValid)
                return View(passwordResetDto);
            try
            {
                var user = await accountService.GetUserByEmailAsync(passwordResetDto.Email);

                var route = Url.Action(ControllerConstants.PasswordResetConfirm, ControllerConstants.Account, null, Request.Scheme);

                var resetMessage = await messageService.CreateResetMessageAsync(user, route!);

                await emailService.SendEmailAsync(resetMessage);

            }
            catch (NotFoundException) { } 
                

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.PasswordResetConfirmMessage;

            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(ControllerConstants.Profile);

            return View(passwordResetDto);
        }

        [HttpGet]
        [Route(ControllerConstants.PasswordResetConfirmEndpoint)]
        public IActionResult PasswordResetConfirm([FromQuery] string email, [FromQuery] string token)
        {
            return View(new PasswordResetConfirmDto { Email = email, Token = token });
        }

        [HttpPost]
        [Route(ControllerConstants.PasswordResetConfirmEndpoint)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordResetConfirm(PasswordResetConfirmDto passwordResetConfirm)
        {
            if (!ModelState.IsValid)
                return View(passwordResetConfirm);

            await accountService.ResetPasswordAsync(passwordResetConfirm);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.PasswordHasBeenReset;

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

        }

        [HttpGet]
        [Route(ControllerConstants.Profile)]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string username = User.Identity?.Name!;

            var user = await accountService.GetUserByUserNameAsync(username);

            return View(ControllerConstants.Profile,user);
        }

        [HttpPost]
        [Route(ControllerConstants.ProfileUpdate)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var currentUser = await accountService.GetUserByUserNameAsync(User.Identity?.Name!);
                return View(ControllerConstants.Profile, currentUser);
            }

            string username = User.Identity?.Name!;

            var user = await accountService.UpdateUserAsync(username, userDto);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.ProfileHasBeenUpdated;

            return RedirectToAction(ControllerConstants.Profile);
        }

    }
}
