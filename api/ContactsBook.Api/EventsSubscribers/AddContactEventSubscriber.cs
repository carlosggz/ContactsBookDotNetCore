
using ContactsBook.Common.Events;
using ContactsBook.Domain.Contacts;

namespace ContactsBook.Api.EventsSubscribers
{
    [EventSubscriber(typeof(ContactAddedDomainEvent))]
    public class AddContactEventSubscriber : IDomainEventSubscriber<ContactAddedDomainEvent>
    {
        public void Dispatch(ContactAddedDomainEvent domainEvent)
        {
            System.Diagnostics.Debug.WriteLine(domainEvent.AggregateRootId);
        }
    }
}
