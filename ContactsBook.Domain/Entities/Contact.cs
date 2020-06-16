using ContactsBook.Common.Entities;
using ContactsBook.Common.Events;
using ContactsBook.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Entities
{
    public class Contact : IAggregateRoot
    {
        public IdValueObject Id { get; set; }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void RecordEvent(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }
    }
}
