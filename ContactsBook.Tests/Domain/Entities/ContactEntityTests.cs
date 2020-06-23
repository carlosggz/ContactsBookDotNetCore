using ContactsBook.Common.Exceptions;
using ContactsBook.Tests.Domain.ValueObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Domain.Entities
{
    [TestFixture]
    public class ContactEntityTests
    {
        [Test]
        public void CreateNewContactContainsNoDependencies()
        {
            var contact = ObjectMotherContact.Random();

            Assert.NotNull(contact);
            Assert.AreEqual(0, contact.EmailAddresses.Count);
            Assert.AreEqual(0, contact.PhoneNumbers.Count);
        }

        [Test]
        public void ContactAddEmailAddressesAddsMailAddresses()
        {
            var contact = ObjectMotherContact.Random();
            var email1 = ObjectMotherEmailValueObject.Random();
            var email2 = ObjectMotherEmailValueObject.Random();

            contact.AddEmailAddress(email1);
            contact.AddEmailAddress(email2);
            
            Assert.AreEqual(2, contact.EmailAddresses.Count);
            Assert.IsTrue(contact.EmailAddresses.Contains(email1));
            Assert.IsTrue(contact.EmailAddresses.Contains(email2));
        }

        [Test]
        public void ContactRemoveEmailAddressesRemovesMailAddresses()
        {
            var contact = ObjectMotherContact.Random();
            var email1 = ObjectMotherEmailValueObject.Random();
            var email2 = ObjectMotherEmailValueObject.Random();
            contact.AddEmailAddress(email1);
            contact.AddEmailAddress(email2);

            contact.RemoveEmailAddress(email1);

            Assert.AreEqual(1, contact.EmailAddresses.Count);
            Assert.IsTrue(contact.EmailAddresses.Contains(email2));
        }

        [Test]
        public void ContactAddPhoneNumbersAddsPhoneNumbers()
        {
            var contact = ObjectMotherContact.Random();
            var phone1 = ObjectMotherPhoneValueObject.Random();
            var phone2 = ObjectMotherPhoneValueObject.Random();

            contact.AddPhoneNumber(phone1);
            contact.AddPhoneNumber(phone2);

            Assert.AreEqual(2, contact.PhoneNumbers.Count);
            Assert.IsTrue(contact.PhoneNumbers.Contains(phone1));
            Assert.IsTrue(contact.PhoneNumbers.Contains(phone2));
        }

        [Test]
        public void ContactRemovePhoneNumbersRemovesPhoneNumbers()
        {
            var contact = ObjectMotherContact.Random();
            var phone1 = ObjectMotherPhoneValueObject.Random();
            var phone2 = ObjectMotherPhoneValueObject.Random();
            contact.AddPhoneNumber(phone1);
            contact.AddPhoneNumber(phone2);

            contact.RemovePhoneNumber(phone1);

            Assert.AreEqual(1, contact.PhoneNumbers.Count);
            Assert.IsTrue(contact.PhoneNumbers.Contains(phone2));
        }

        [Test]
        public void ContactSetInvalidNameThrowsException()
            => Assert.Throws<DomainException>(() => ObjectMotherContact.Random().Name = null);
    }
}
