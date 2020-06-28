using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Events
{
    public interface IDomainEventSubscriber<T> where T: IDomainEvent
    {
        void Dispatch(T domainEvent);
    }
}
