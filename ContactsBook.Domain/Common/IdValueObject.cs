using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Utils;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Common
{
    public class IdValueObject : ValueObject
    {
        public string Value { get; private set; }

        public IdValueObject()
            : this(Guid.NewGuid().ToString())
        { }

        public IdValueObject(string value)
        {
            if (!ValidationHelper.IsValidId(value))
                throw new DomainException("Invalid Id");

            Value = value;
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
