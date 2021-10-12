using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        private string Key = String.Empty;
        private string User = string.Empty;


        public EmailSender(IConfiguration config)
        {
            _config = config;

            Key = config["SendGrid:key"];
            User = config["SendGrid:User"];
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            var client = new SendGridClient(Key);

            var sendGridMessage = new SendGridMessage()
            {
                From = new EmailAddress("scott.roberts@virgin.net", User),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            sendGridMessage.AddTo(emailAddress);
            sendGridMessage.SetClickTracking(false, false);

            await client.SendEmailAsync(sendGridMessage);
        }
    }
}
