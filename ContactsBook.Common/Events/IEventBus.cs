using System.Collections.Generic;

namespace ContactsBook.Common.Events
{
    public interface IEventBus
    {
        void Publish(IEnumerable<IDomainEvent> domainEvents);
    }

}
