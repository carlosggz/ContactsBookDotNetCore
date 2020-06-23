using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;


namespace ContactsBook.Domain.Contacts
{
    public class ContactAddedDomainEvent : DomainEvent
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public override string EventName => "Contact Added";

        public ContactAddedDomainEvent(ContactEntity contact)
            : base(contact?.Id.ToString())
        {
            if (contact == null)
                throw new DomainException("Invalid entity");

            FirstName = contact.Name.FirstName;
            LastName = contact.Name.LastName;
        }
    }
}
