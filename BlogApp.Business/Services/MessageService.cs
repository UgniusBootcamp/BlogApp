using BlogApp.Business.Interfaces;
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
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = String.Format(
                "<button style='color:#0055cc; font-size:24px;' onclick=\"window.location.href='{0}'\">Confirm Email</button>", callback);

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
            string body = String.Format(
                "<button style='color:#0055cc; font-size:24px;' onclick=\"window.location.href='{0}'\">Reset Password</button>", callback);

            var message = new Message([user.Email!], "Password Reset", body);
            return message;
        }
    }
}
