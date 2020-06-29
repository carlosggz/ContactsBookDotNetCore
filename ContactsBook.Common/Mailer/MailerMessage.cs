using ContactsBook.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ContactsBook.Common.Mailer
{
    public class MailerMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailerRecipient From { get; set; }
        public IEnumerable<MailerRecipient> To { get; set; }
        public IEnumerable<MailerRecipient> CC { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Subject))
                throw new InvalidParametersException("Invalid subject");

            if (string.IsNullOrWhiteSpace(Body))
                throw new InvalidParametersException("Invalid body");

            if (From == null)
                throw new InvalidParametersException("Invalid from");

            if (To.Any(x => x == null))
                throw new InvalidParametersException("Invalid TO address");

            if (CC.Any(x => x == null))
                throw new InvalidParametersException("Invalid CC address");
        }
    }
}
