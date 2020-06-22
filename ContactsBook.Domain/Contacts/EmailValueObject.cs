using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Utils;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactsBook.Domain.Contacts
{
    public class EmailValueObject : ValueObject
    {
        public string Value { get; private set; }

        public EmailValueObject(string emailAddress)
        {
            if (!ValidationHelper.IsValidEmail(emailAddress))
                throw new DomainException("Invalid email address");

            Value = emailAddress;
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
