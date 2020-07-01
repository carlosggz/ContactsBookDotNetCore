using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ContactsBook.Tests.Application.ContactService
{
    [TestFixture]
    public class UpdateContactTests : ContactBaseTest
    {
        [Test]
        public void UpdateContactCallsCollaborators()
        {
            var contact = ContactEntityObjectMother.Random();
            var model = ContactsModelObjectMother.FromEntity(contact);

            _repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            _repo.Setup(x => x.ExistsContactWithName(contact.Name, contact.Id)).Returns(false);
            _repo.Setup(x => x.Update(It.IsAny<ContactEntity>()));

            _uof.Setup(x => x.StartChanges());
            _uof.Setup(x => x.CommitChanges());

            _eventBus.Setup(x => x.Record(It.Is<ContactUpdatedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            _eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            _contactsService.UpdateContact(model);

            _repo.VerifyAll();
            _uof.VerifyAll();
            _eventBus.VerifyAll();
        }

        [Test]
        public void UpdateContactWithNullValuesThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _contactsService.UpdateContact(null));
        }

        [Test]
        public void UpdateContactWithInvalidValuesThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            model.FirstName = null;

            Assert.Throws<EntityValidationException>(() => _contactsService.UpdateContact(model));
        }

        [Test]
        public void UpdateRepeatedContactThrowsException()
        {
            var original = ContactEntityObjectMother.Random();
            var duplicated = ContactEntityObjectMother.Random();
            duplicated.Name = original.Name;

            _repo.Setup(x => x.ExistsContactWithName(duplicated.Name, duplicated.Id)).Returns(true);

            var model = ContactsModelObjectMother.FromEntity(duplicated);

            Assert.Throws<DomainException>(() => _contactsService.UpdateContact(model));
        }

        [Test]
        public void UpdateSameContactDoesNotThrowsException()
        {
            var contact = ContactEntityObjectMother.Random();
            var model = ContactsModelObjectMother.FromEntity(contact);

            _repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            _repo.Setup(x => x.ExistsContactWithName(contact.Name, contact.Id)).Returns(false);
            _repo.Setup(x => x.Update(It.IsAny<ContactEntity>()));

            _uof.Setup(x => x.StartChanges());
            _uof.Setup(x => x.CommitChanges());

            _eventBus.Setup(x => x.Record(It.Is<ContactUpdatedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            _eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            _contactsService.UpdateContact(model);

            _repo.VerifyAll();
            _uof.VerifyAll();
            _eventBus.VerifyAll();
        }
    }
}
