using ContactsBook.Application.Services.GetContact;
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
            repo.Setup(x => x.GetById(contact.Id)).Returns(contact);

            var cmd = new GetContactQuery(contact.Id.Value);
            var handler = new GetContactQueryHandler(uow.Object, eventBus.Object, repo.Object);

            var model = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;

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

            repo.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        { 
            var handler = new GetContactQueryHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<DomainException>(() => handler.Handle(new GetContactQuery(null), new System.Threading.CancellationToken()));
            Assert.Throws<DomainException>(() => handler.Handle(new GetContactQuery(string.Empty), new System.Threading.CancellationToken()));
            Assert.Throws<DomainException>(() => handler.Handle(new GetContactQuery("123"), new System.Threading.CancellationToken()));
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            var handler = new GetContactQueryHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<EntityNotFound>(() => handler.Handle(new GetContactQuery(new IdValueObject().Value), new System.Threading.CancellationToken()));
        }
    }
}
