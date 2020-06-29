using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Mailer;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Infrastructure.Mailer
{
    public class SmtpMailer : IMailer
    {
        private readonly SmtpConfiguration _config;
        public SmtpMailer(SmtpConfiguration config) => _config = config;

        public void SendAsync(MailerMessage message)
        {
            ValidateMessage(message);
            var toSend = BuildMessage(message);
            var client = GetClient();

            client.SendAsync(toSend, string.Empty);
        }

        private void ValidateMessage(MailerMessage message)
        {
            if (message == null)
                throw new InvalidParametersException("Invalid message");

            message.Validate();
        }
        private static MailMessage BuildMessage(MailerMessage message)
        {
            var toSend = new MailMessage
            {
                From = new MailAddress(message.From.MailAddress, message.From.Name),
                Body = message.Body,
                IsBodyHtml = true,
                Subject = message.Subject
            };

            foreach (var to in message.To)
                toSend.To.Add(to.MailAddress);

            if (message.CC != null && message.CC.Any())
            {
                foreach (var cc in message.CC)
                    toSend.CC.Add(cc.MailAddress);
            }

            return toSend;
        }

        private SmtpClient GetClient() 
            => new SmtpClient(_config.Host, _config.Port)
                {
                    Credentials = new NetworkCredential(_config.User, _config.Password),
                    EnableSsl = _config.UseSsl
                };
    }
}
