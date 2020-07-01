using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Tests.Common.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Application.ContactService
{
    [TestFixture]
    public class GetContactTests: ContactBaseTest
    {
        [Test]
        public void ExistingContactReturnsEntity()
        {
            var contact = ContactEntityObjectMother.Random();
            _repo.Setup(x => x.GetById(contact.Id)).Returns(contact);

            var model = _contactsService.GetContact(contact.Id.Value);

            Assert.IsNotNull(model);
            Assert.AreEqual(contact.Id.Value, model.Id);
            Assert.AreEqual(contact.Name.FirstName, model.FirstName);
            Assert.AreEqual(contact.Name.LastName, model.LastName);
            Assert.AreEqual(contact.EmailAddresses.Count, model.EmailAddresses.Count());
            Assert.AreEqual(contact.PhoneNumbers.Count, model.PhoneNumbers.Count());

            foreach (var email in contact.EmailAddresses)
                Assert.IsTrue(model.EmailAddresses.Contains(email.Value)) ;

            foreach (var phone in contact.PhoneNumbers)
                Assert.IsTrue(model.PhoneNumbers.Any(x => x.PhoneType == phone.PhoneType && x.PhoneNumber == phone.PhoneNumber));

            _repo.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        {
            Assert.Throws<DomainException>(() => _contactsService.GetContact(null));
            Assert.Throws<DomainException>(() => _contactsService.GetContact(string.Empty));
            Assert.Throws<DomainException>(() => _contactsService.GetContact("123"));
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            Assert.Throws<EntityNotFound>(() => _contactsService.GetContact(new IdValueObject().Value));
        }
    }
}
