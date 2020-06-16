using System.Collections.Generic;

namespace ContactsBook.Common.Events
{
    public interface IQueryBus
    {
        void Publish(IEnumerable<IDomainEvent> domainEvents);
    }

}
