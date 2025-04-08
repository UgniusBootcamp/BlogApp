using BlogApp.Data.Helpers.Email;

namespace BlogApp.Business.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="message">message to send</param>
        Task SendEmailAsync(Message message);
    }
}
