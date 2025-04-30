using System.Security.Claims;

namespace BlogApp.Business.Interfaces
{
    public interface IJWTService
    {
        /// <summary>
        /// Method to create access token
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="userId">user id</param>
        /// <param name="roles">user roles</param>
        /// <returns>access token</returns>
        public string CreateAccessToken(string username, string userId, IEnumerable<string> roles);

        /// <summary>
        /// Method to create refresh token
        /// </summary>
        /// <param name="sessionsId">session id</param>
        /// <param name="userId">user id</param>
        /// <returns>refresh token</returns>
        public string CreateRefreshToken(string userId);

        /// <summary>
        /// method for parsing refresh token
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <param name="claims">claims</param>
        /// <returns>true if parse successful, false otherwise</returns>
        public bool TryParseRefreshToken(string refreshToken, out ClaimsPrincipal? claims);
    }
}
