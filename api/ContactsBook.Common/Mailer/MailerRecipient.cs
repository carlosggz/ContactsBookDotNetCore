using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Utils;

namespace ContactsBook.Common.Mailer
{
    public class MailerRecipient
    {
        public string Name { get; private set; }
        public string MailAddress { get; private set; }

        public MailerRecipient(string name, string address)
        {
            if (!ValidationHelper.IsValidEmail(address))
                throw new InvalidParametersException("Invalid mail address");

            Name = name;
            MailAddress = address;
        }
    }
}
