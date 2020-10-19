using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Application.Services.DeleteContact;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
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

            repo.Setup(x => x.GetById(contact.Id)).Returns(contact);
            repo.Setup(x => x.Delete(It.Is<IdValueObject>(p => p == contact.Id)));

            uow.Setup(x => x.StartChanges());
            uow.Setup(x => x.CommitChanges());

            eventBus.Setup(
                x => x.Record(It.Is<ContactDeletedDomainEvent>(
                    p => p.FirstName == contact.Name.FirstName && p.LastName == contact.Name.LastName && p.AggregateRootId == contact.Id.Value)));
            eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            var cmd = new DeleteContactCommand(contact.Id.Value);
            var handler = new DeleteContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            var x = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;


            repo.VerifyAll();
            uow.VerifyAll();
            eventBus.VerifyAll();
        }

        [Test]
        public void DeleteWithInvalidIdThrowException()
        {
            var handler = new DeleteContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<InvalidEntityException>(() => handler.Handle(new DeleteContactCommand(null), new System.Threading.CancellationToken()));
            Assert.Throws<InvalidEntityException>(() => handler.Handle(new DeleteContactCommand(string.Empty), new System.Threading.CancellationToken()));
            Assert.Throws<DomainException>(() => handler.Handle(new DeleteContactCommand("abc"), new System.Threading.CancellationToken()));
        }

        [Test]
        public void DeleteWithInvalidContactThrowException()
        {
            var id = new IdValueObject();
            repo.Setup(x => x.GetById(It.Is<IdValueObject>(p => p == id))).Returns<ContactEntity>(null);
            var cmd = new DeleteContactCommand(id.Value);
            var handler = new DeleteContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<EntityNotFound>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
            repo.VerifyAll();
        }
    }
}
