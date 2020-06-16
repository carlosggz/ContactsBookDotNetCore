using ContactsBook.Common.Entities;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class Contact : BaseAggregateRoot
    {
        public IdValueObject Id { get; set; }
        public ContactNameValueObject Name { get; private set; }
        private readonly List<EmailValueObject> _emailAddresses;
        public readonly List<PhoneValueObject> _phoneNumbers;

        public Contact(IdValueObject id, ContactNameValueObject name)
        {
            if (id == null || name == null)
                throw new DomainException("Invalid paramaters");

            Id = id;
            Name = name;
            _emailAddresses = new List<EmailValueObject>();
            _phoneNumbers = new List<PhoneValueObject>();
        }

        #region Email Address management
        public void AddEmailAddress(EmailValueObject email)
        {
            if (email == null || _emailAddresses.Any(x => x == email))
                return;

            _emailAddresses.Add(email);
        }

        public void RemoveEmailAddress(EmailValueObject email)
        {
            if (email == null || !_emailAddresses.Any(x => x == email))
                return;

            _emailAddresses.Remove(_emailAddresses.FirstOrDefault(x => x == email));
        }

        public IReadOnlyCollection<EmailValueObject> EmailAddresses => _emailAddresses;

        #endregion

        #region Phone numbers management
        public void AddPhoneNumber(PhoneValueObject phone)
        {
            if (phone == null || _phoneNumbers.Any(x => x == phone))
                return;

            _phoneNumbers.Add(phone);
        }

        public void RemovePhoneNumber(PhoneValueObject phone)
        {
            if (phone == null || !_phoneNumbers.Any(x => x == phone))
                return;

            _phoneNumbers.Remove(_phoneNumbers.FirstOrDefault(x => x == phone));
        }

        public IReadOnlyCollection<PhoneValueObject> PhoneNumbers => _phoneNumbers;

        #endregion
    }
}
