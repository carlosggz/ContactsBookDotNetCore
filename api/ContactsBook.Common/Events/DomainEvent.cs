using ContactsBook.Common.Exceptions;
using System;

namespace ContactsBook.Common.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public string EventId { get; private set; }
        public abstract string EventName { get; }
        public string AggregateRootId { get; private set; }
        public DateTime OccurrenceDate { get; private set; }

        public DomainEvent(string aggregateRootId)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId))
                throw new InvalidEventDomainException("Invalid domain event");

            EventId = Guid.NewGuid().ToString();
            AggregateRootId = aggregateRootId;
            OccurrenceDate = DateTime.Now;
        }
    }

}
