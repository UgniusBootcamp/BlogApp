using System.Runtime.CompilerServices;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;
using BlogApp.Data.Helpers.Settings;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogApp.Business.Services
{
    public class EmailService(
        IOptions<EmailConfiguration> emailConfig
        ) : IEmailService
    {
        private readonly EmailConfiguration _emailConfig = emailConfig.Value;

        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="message">message</param>
        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        /// <summary>
        /// Method for email creation
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>message</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = String.Format(ServiceConstants.MessageBody, message.Content)
            };

            return emailMessage;
        }

        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="message">message</param>
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
