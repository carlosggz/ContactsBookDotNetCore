using System.Collections.Generic;

namespace ContactsBook.Common.Events
{
    public interface IDomainBus
    {
        void Publish(IEnumerable<IDomainEvent> domainEvents);
    }

}
