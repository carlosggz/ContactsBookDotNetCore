using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ContactsBook.Tests.Application.ContactService
{
    [TestFixture]
    public class DeleteContactTests : ContactBaseTest
    {
        [Test]
        public void DeleteContactCallsCollaborators()
        {
            var contact = ContactEntityObjectMother.Random();

            _repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            _repo.Setup(x => x.Delete(It.IsAny<ContactEntity>()));

            _uof.Setup(x => x.StartChanges());
            _uof.Setup(x => x.CommitChanges());

            _eventBus.Setup(
                x => x.Record(It.Is<ContactDeletedDomainEvent>(
                    p => p.FirstName == contact.Name.FirstName && p.LastName == contact.Name.LastName && p.AggregateRootId == contact.Id.Value)));
            _eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            _contactsService.DeleteContact(contact.Id.Value);

            _repo.VerifyAll();
            _uof.VerifyAll();
            _eventBus.VerifyAll();
        }

        [Test]
        public void DeleteWithInvalidIdThrowException()
        {
            Assert.Throws<InvalidEntityException>(() => _contactsService.DeleteContact(null));
            Assert.Throws<InvalidEntityException>(() => _contactsService.DeleteContact(""));
            Assert.Throws<DomainException>(() => _contactsService.DeleteContact("abc"));
        }

        [Test]
        public void DeleteWithInvalidContactThrowException()
        {
            var id = new IdValueObject();
            _repo.Setup(x => x.GetById(It.Is<IdValueObject>(p => p == id))).Returns<ContactEntity>(null);

            Assert.Throws<InvalidEntityException>(() => _contactsService.DeleteContact(id.Value));
            _repo.VerifyAll();
        }
    }
}
