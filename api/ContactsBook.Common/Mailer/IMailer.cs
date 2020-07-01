using System;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Common.Mailer
{
    public interface IMailer
    {
        void SendAsync(MailerMessage message);
    }
}
