using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContactsBook.Tests.Application.ContactService
{
    [TestFixture]
    public class AddContactTests : ContactBaseTest
    {
        [Test]
        public void AddContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();

            _repo.Setup(x => x.Add(It.IsAny<ContactEntity>()));

            _uof.Setup(x => x.StartChanges());
            _uof.Setup(x => x.CommitChanges());

            _eventBus.Setup(x => x.Record(It.Is<ContactAddedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            _eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            _contactsService.AddContact(model);

            _repo.VerifyAll();
            _uof.VerifyAll();
            _eventBus.VerifyAll();
        }

        [Test]
        public void AddContactWithNullValuesThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _contactsService.AddContact(null));
        }

        [Test]
        public void AddContactWithInvalidValuesThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            model.FirstName = null;

            Assert.Throws<EntityValidationException>(() => _contactsService.AddContact(model));
        }

        [Test]
        public void AddRepeatedContactThrowsException()
        {
            var original = ContactEntityObjectMother.Random();
            var duplicated = ContactEntityObjectMother.Random();
            duplicated.Name = original.Name;

            _repo.Setup(x => x.ExistsContactWithName(duplicated.Name, null)).Returns(true);

            var model = ContactsModelObjectMother.FromEntity(duplicated);

            Assert.Throws<DomainException>(() => _contactsService.AddContact(model));
        }
    }
}
