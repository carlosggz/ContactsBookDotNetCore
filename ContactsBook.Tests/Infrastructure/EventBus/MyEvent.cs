using ContactsBook.Common.Events;

namespace ContactsBook.Tests.Infrastructure.EventBus
{
    public class MyEvent : DomainEvent
    {
        public MyEvent(string aggregateRootId) 
            : base(aggregateRootId)
        {}

        public override string EventName => "Test event";
    }

}
