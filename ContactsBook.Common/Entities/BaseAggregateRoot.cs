using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Entities
{
    public abstract class BaseAggregateRoot: IAggregateRoot
    {
        private List<IDomainEvent> _events = new List<IDomainEvent>();

        protected void RecordEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new InvalidEventDomainException("Invalid domain event");

            _events.Add(domainEvent);
        }

        public IEnumerable<IDomainEvent> PullEvents()
        {
            var toReturn = new List<IDomainEvent>(_events);
            _events.Clear();
            return toReturn;
        }
    }
}
