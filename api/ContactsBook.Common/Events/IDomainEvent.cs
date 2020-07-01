using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Events
{
    public interface IDomainEvent
    {
        string EventId { get; }
        string AggregateRootId { get; }
        DateTime OccurrenceDate { get; }
    }

}
