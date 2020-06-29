using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EventBus
{
    [TestFixture]
    public class EventBusTests
    {
        IDictionary<Type, Type>  _dict = new Dictionary<Type, Type>() { { typeof(MySubscriber), typeof(MyEvent) } };
        ContactsBook.Infrastructure.EventsBus.EventBus _bus;
        MyEvent _event;

        [SetUp]
        public void Init()
        {
            var contact = ContactEntityObjectMother.Random();
            _event = new MyEvent(contact.Id.Value);            
            _bus = new ContactsBook.Infrastructure.EventsBus.EventBus(_dict);
            MySubscriber.DomainEvent = null;
        }

        [Test]
        public void PublishCallsEventsSubscribers()
        {
            _bus.Record(_event);

            _bus.PublishAsync().Wait();

            Assert.IsNotNull(MySubscriber.DomainEvent);
            Assert.AreEqual(_event.AggregateRootId, MySubscriber.DomainEvent.AggregateRootId);
            Assert.AreEqual(_event.EventId, MySubscriber.DomainEvent.EventId);
            Assert.AreEqual(_event.EventName, MySubscriber.DomainEvent.EventName);
            Assert.AreEqual(_event.OccurrenceDate, MySubscriber.DomainEvent.OccurrenceDate);
        }

        [Test]
        public void PublishWithoutEventsDoesNotCallsEventsSubscribers()
        {
            _bus.PublishAsync().Wait();

            Assert.IsNull(MySubscriber.DomainEvent);
        }

        [Test]
        public void DiscardEventsRemoveAllPendingEvents()
        {
            _bus.Record(_event);

            _bus.DiscardEvents();
            _bus.PublishAsync().Wait();

            Assert.IsNull(MySubscriber.DomainEvent);
        }
    }

}
