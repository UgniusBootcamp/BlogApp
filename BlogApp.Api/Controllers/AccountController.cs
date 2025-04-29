using BlogApp.Business.Interfaces;
using BlogApp.Business.Services;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Helpers.Settings;
using BlogApp.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogApp.Api.Controllers
{
    [ApiController]
    [Route(ControllerConstants.api + "[controller]")]
    public class AccountController(
         IOptions<JwtSettings> jwtSettings,
         IAccountService accountService
        ) : Controller
    {

        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        /// <summary>
        /// Endpoint for login
        /// </summary>
        /// <param name="loginDto">login data</param>
        /// <returns>access token</returns>
        /// <response code="200">successful login</response>>
        /// <response code="422">login incorrect</response>>
        [HttpPost]
        [Route(ControllerConstants.LoginApi)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var login = await accountService.LoginApi(loginDto);

            var refreshToken = await accountService.CreateRefreshTokenAsync(login.UserId);

            UpdateCookie(refreshToken);

            return Ok(ApiResponse.OkResponse("", new { AccessToken = login.Token }));
        }

        /// <summary>
        /// Ednpoint for access token refresh
        /// </summary>
        /// <returns>refreshed access token</returns>
        /// <response code="200">new access token</response>>
        /// <response code="422">If refresh not found or invalid or session invalid</response>>
        [HttpPost]
        [Route(ControllerConstants.AccessTokenApi)]
        public async Task<IActionResult> AccessToken()
        {
            HttpContext.Request.Cookies.TryGetValue(ControllerConstants.RefreshToken, out var refreshToken);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return UnprocessableEntity(ApiResponse.UnprocessableEntityResponse(ControllerConstants.RefreshTokenNotFound));
            }


            var login = await accountService.GetAccessTokenFromRefreshToken(refreshToken);

            var newRefreshToken = await accountService.CreateRefreshTokenAsync(login.UserId);

            UpdateCookie(newRefreshToken);

            return Ok(ApiResponse.OkResponse(ControllerConstants.AccessTokenRefreshed, new { AccessToken = login.Token }));

        }

        /// <summary>
        /// Method for refresh token update in cookies
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        private void UpdateCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Secure = true
            };

            Response.Cookies.Append(ControllerConstants.RefreshToken, refreshToken, cookieOptions);
        }
    }
}
