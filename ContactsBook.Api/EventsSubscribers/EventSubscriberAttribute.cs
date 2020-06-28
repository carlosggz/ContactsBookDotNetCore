using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsBook.Api.EventsSubscribers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventSubscriberAttribute : Attribute
    {
        public EventSubscriberAttribute(Type domainEventType)
        {
            DomainEventType = domainEventType;
        }

        public Type DomainEventType { get; }
    }
}
