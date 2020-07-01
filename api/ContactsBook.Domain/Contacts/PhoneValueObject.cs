using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Utils;
using ContactsBook.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactsBook.Domain.Contacts
{
    public class PhoneValueObject : ValueObject
    {
        public PhoneType PhoneType { get; private set; }
        public string PhoneNumber { get; private set; }
        public PhoneValueObject(PhoneType phoneType, string phoneNumber)
        {
            if (!ValidationHelper.IsValidPhoneNumber(phoneNumber))
                throw new DomainException("Invalid phone number");

            if (phoneNumber.Length > 20)
                throw new DomainException("Phone number cannot exceed 20 characters");

            PhoneType = phoneType;
            PhoneNumber = phoneNumber;
        }

        public override string ToString() => $"{PhoneType} {PhoneNumber}";

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PhoneType;
            yield return PhoneNumber;
        }
    }
}
