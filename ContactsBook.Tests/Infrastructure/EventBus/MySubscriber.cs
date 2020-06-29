using ContactsBook.Common.Events;

namespace ContactsBook.Tests.Infrastructure.EventBus
{
    public class MySubscriber : IDomainEventSubscriber<MyEvent>
    {
        public static MyEvent DomainEvent { get; set; } = null;
        public void Dispatch(MyEvent domainEvent) => DomainEvent = domainEvent;
    }

}
