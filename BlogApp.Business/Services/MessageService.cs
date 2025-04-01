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
        public async Task<Message> CreateConfirmationMessageAsync(User user, string uri)
        {
            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(uri))
                throw new ArgumentNullException(ServiceConstants.ArgumentsCannotBeNull);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = CallbackButtonHtml(callback, "Confirm Email");

            var message = new Message([user.Email!], "Email Confirmation", body);

            return message;
        }

        public async Task<Message> CreateResetMessageAsync(User user, string uri)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = CallbackButtonHtml(callback, "Reset Password");

            var message = new Message([user.Email!], "Password Reset", body);
            return message;
        }

        private string CallbackButtonHtml(string callback, string name)
        {
            return $@"
                <a href='{callback}' style='
                    display:inline-block;
                    padding:10px 20px;
                    font-size:16px;
                    color:#fff;
                    background-color:#007bff;
                    text-decoration:none;
                    border-radius:5px;
                '>{name}</a>";
        }
    }
}
