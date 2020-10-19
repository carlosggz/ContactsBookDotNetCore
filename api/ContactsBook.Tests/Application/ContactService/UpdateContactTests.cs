using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Application.Services.UpdateContact;
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

            repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            repo.Setup(x => x.ExistsContactWithName(contact.Name, contact.Id)).Returns(false);
            repo.Setup(x => x.Update(It.IsAny<ContactEntity>()));

            uow.Setup(x => x.StartChanges());
            uow.Setup(x => x.CommitChanges());

            eventBus.Setup(x => x.Record(It.Is<ContactUpdatedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            var cmd = new UpdateContactCommand(model);
            var handler = new UpdateContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            var x = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;

            repo.VerifyAll();
            uow.VerifyAll();
            eventBus.VerifyAll();
        }

        [Test]
        public void UpdateContactWithInvalidValuesThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            model.FirstName = null;
            var cmd = new UpdateContactCommand(model);
            var handler = new UpdateContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<EntityValidationException>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
        }

        [Test]
        public void UpdateRepeatedContactThrowsException()
        {
            var original = ContactEntityObjectMother.Random();
            var duplicated = ContactEntityObjectMother.Random();
            duplicated.Name = original.Name;

            repo.Setup(x => x.ExistsContactWithName(duplicated.Name, duplicated.Id)).Returns(true);

            var model = ContactsModelObjectMother.FromEntity(duplicated);

            var cmd = new UpdateContactCommand(model);
            var handler = new UpdateContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<DomainException>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
        }

        [Test]
        public void UpdateSameContactDoesNotThrowsException()
        {
            var contact = ContactEntityObjectMother.Random();
            var model = ContactsModelObjectMother.FromEntity(contact);

            repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            repo.Setup(x => x.ExistsContactWithName(contact.Name, contact.Id)).Returns(false);
            repo.Setup(x => x.Update(It.IsAny<ContactEntity>()));

            uow.Setup(x => x.StartChanges());
            uow.Setup(x => x.CommitChanges());

            eventBus.Setup(x => x.Record(It.Is<ContactUpdatedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            var cmd = new UpdateContactCommand(model);
            var handler = new UpdateContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            var result = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;

            repo.VerifyAll();
            uow.VerifyAll();
            eventBus.VerifyAll();
        }
    }
}
