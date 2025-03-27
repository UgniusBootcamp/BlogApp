using System.Runtime.CompilerServices;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;
using BlogApp.Data.Helpers.Settings;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogApp.Business.Services
{
    public class EmailService(
        IOptions<EmailConfiguration> emailConfig
        ) : IEmailService
    {
        private readonly EmailConfiguration _emailConfig = emailConfig.Value;

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = String.Format(@"
                <html>
                    <body style='font-family:Arial, sans-serif; color:#444;'>
                        <h2 style='color:#0055cc; font-size:24px;'>{0}</h2>
                        <p style='font-size:16px;'>Thanks for using our blog app system!</p>
                    </body>
                </html>", message.Content)
            };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage message) 
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
