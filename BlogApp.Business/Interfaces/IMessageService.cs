using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;

namespace BlogApp.Business.Interfaces
{
    public interface IMessageService
    {
        /// <summary>
        /// Method to create email confirmation message
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="uri">uri to navigate</param>
        /// <returns>message</returns>
        public Task<Message> CreateConfirmationMessageAsync(User user, string uri);

        /// <summary>
        /// Method to create password reset message
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="uri">uri to navigate</param>
        /// <returns>message</returns>
        public Task<Message> CreateResetMessageAsync(User user, string uri);
    }
}
