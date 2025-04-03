using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Data.Helpers.Email
{
    public class Message
    {
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = to.Select(x => new MailboxAddress("", x)).ToList();
            Subject = subject;
            Content = content;
        }

        public List<MailboxAddress> To { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
