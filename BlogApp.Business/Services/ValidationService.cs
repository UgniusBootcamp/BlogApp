using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Helpers.Exceptions;

namespace BlogApp.Business.Services
{
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Method to check if password is valid
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="confirmPassword">confirmed password</param>
        public void ValidateRegisterPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new BusinessRuleValidationException(ServiceConstants.PasswordsDoNotMatch);
            }
        }
    }
}
