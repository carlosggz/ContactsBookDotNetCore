using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Infrastructure.EventsBus
{
    public class EventBus : IEventBus
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        private readonly Dictionary<Type, Type> _subscribers;

        public EventBus(IDictionary<Type, Type> subscribers)
        {
            _subscribers = new Dictionary<Type, Type>(subscribers);
        }

        #region IEventBus

        public void Record(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new InvalidEventDomainException("Invalid event");

            _events.Add(domainEvent);
        }
        public Task PublishAsync()
        {
            return Task.Run(() =>
            {
                foreach (var evt in _events)
                {
                    foreach (var kvp in _subscribers.Where(x => x.Value == evt.GetType()))
                    {
                        try
                        {
                            var subscriber = System.Activator.CreateInstance(kvp.Key);
                            var method = kvp.Key.GetMethod("Dispatch");
                            method.Invoke(subscriber, new object[] { evt });
                        }
                        catch (Exception)
                        {}
                    }
                }

                _events.Clear();
            });
        }

        public void DiscardEvents()
        {
            _events.Clear();
        } 

        #endregion
    }
}
