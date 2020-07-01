using ContactsBook.Common.Mailer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Mailer
{
    public class NullMailer : IMailer
    {
        public void SendAsync(MailerMessage message)
        {
        }
    }
}
