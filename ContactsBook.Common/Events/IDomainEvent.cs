using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Events
{
    public interface IDomainEvent
    {
        string AggregateRootId { get; }
        Object EventDetails { get; }
    }

}
