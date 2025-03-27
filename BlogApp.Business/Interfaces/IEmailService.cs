using BlogApp.Data.Helpers.Email;

namespace BlogApp.Business.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
