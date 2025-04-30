using BlogApp.Business.Interfaces;
using BlogApp.Data.Helpers.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public class JWTService(IOptions<JwtSettings> jwtSettings) : IJWTService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly SymmetricSecurityKey _authSigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));

        /// <summary>
        /// Method to create access token
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="userId">user id</param>
        /// <param name="roles">user roles</param>
        /// <returns>access token</returns>
        public string CreateAccessToken(string username, string userId, IEnumerable<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, username),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, userId)
            };

            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Method to create refresh token
        /// </summary>
        /// <param name="sessionsId">session id</param>
        /// <param name="userId">user id</param>
        /// <returns>refresh token</returns>
        public string CreateRefreshToken(string userId)
        {
            var authClaims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, userId),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// method for parsing refresh token
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <param name="claims">claims</param>
        /// <returns>true if parse successful, false otherwise</returns>
        public bool TryParseRefreshToken(string refreshToken, out ClaimsPrincipal? claims)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler() { MapInboundClaims = false };

                var validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = _authSigingKey,
                    ValidateLifetime = true
                };

                claims = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);

                return true;
            }
            catch (Exception)
            {
                claims = null;
                return false;
            }
        }
    }
}
