using ContactsBook.Common.Exceptions;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class ContactNameValueObject : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public ContactNameValueObject(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Invalid first name or last name");

            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString() => $"{FirstName} {LastName}";

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Return each element one at a time
            yield return FirstName;
            yield return LastName;
        }
    }
}
