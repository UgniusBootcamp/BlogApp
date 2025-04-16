using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BlogApp.Business.Services
{
    public class MessageService(UserManager<User> userManager) : IMessageService
    {
        /// <summary>
        /// Method to create email confirmation message
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="uri">confirmation uri</param>
        /// <returns>message</returns>
        /// <exception cref="ArgumentNullException">argument is null</exception>
        public async Task<Message> CreateConfirmationMessageAsync(User user, string uri)
        {
            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(uri))
                throw new ArgumentNullException(ServiceConstants.ArgumentsCannotBeNull);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {ServiceConstants.Token, token },
                {ServiceConstants.Email, user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = CallbackButtonHtml(callback, ServiceConstants.ConfirmEmail);

            var message = new Message([user.Email!], ServiceConstants.EmailConfirmation, body);

            return message;
        }

        /// <summary>
        /// Method to create password reset message
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="uri">reset uri</param>
        /// <returns>message</returns>
        public async Task<Message> CreateResetMessageAsync(User user, string uri)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {ServiceConstants.Token, token },
                {ServiceConstants.Email, user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = CallbackButtonHtml(callback, ServiceConstants.ResetPassword);

            var message = new Message([user.Email!], ServiceConstants.PasswordReset, body);
            return message;
        }

        /// <summary>
        /// Method to create callback button html
        /// </summary>
        /// <param name="callback">callback</param>
        /// <param name="name">button name</param>
        /// <returns>button in html</returns>
        private string CallbackButtonHtml(string callback, string name)
        {
            return String.Format(ServiceConstants.Button, callback, name);
        }
    }
}
