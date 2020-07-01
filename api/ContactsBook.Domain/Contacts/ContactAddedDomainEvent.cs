using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;


namespace ContactsBook.Domain.Contacts
{
    public class ContactAddedDomainEvent : DomainEvent
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public override string EventName => "Contact Added";

        public ContactAddedDomainEvent(string id, string firstName, string lastName)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Invalid entity");

            FirstName = firstName;
            LastName = lastName;
        }
    }
}
