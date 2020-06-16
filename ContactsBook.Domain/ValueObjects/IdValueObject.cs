using ContactsBook.Common.Exceptions;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.ValueObjects
{
    public class IdValueObject : IValueObject
    {
        public string Value { get; private set; }

        public IdValueObject(string value)
        {
            if (!Guid.TryParse(value, out Guid uid))
                throw new DomainException("Invalid Id");

            Value = uid.ToString();
        }

        public static IdValueObject Generate() => new IdValueObject(Guid.NewGuid().ToString());
    }
}
