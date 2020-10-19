using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Application.Services.AddContact;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
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

            repo.Setup(x => x.Add(It.IsAny<ContactEntity>()));

            uow.Setup(x => x.StartChanges());
            uow.Setup(x => x.CommitChanges());

            eventBus.Setup(x => x.Record(It.Is<ContactAddedDomainEvent>(p => p.FirstName == model.FirstName && p.LastName == model.LastName && p.AggregateRootId == model.Id)));
            eventBus.Setup(x => x.PublishAsync()).Returns(Task.Delay(500));

            var cmd = new AddContactCommand(model);
            var handler = new AddContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            var x = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;

            repo.VerifyAll();
            uow.VerifyAll();
            eventBus.VerifyAll();
        }

        [Test]
        public void AddContactWithNullValuesThrowsException()
        {
            var cmd = new AddContactCommand(null);
            var handler = new AddContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<InvalidEntityException>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
        }

        [Test]
        public void AddContactWithInvalidValuesThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            model.FirstName = null;
            var cmd = new AddContactCommand(model);
            var handler = new AddContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<EntityValidationException>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
        }

        [Test]
        public void AddRepeatedContactThrowsException()
        {
            var original = ContactEntityObjectMother.Random();
            var duplicated = ContactEntityObjectMother.Random();
            duplicated.Name = original.Name;

            repo.Setup(x => x.ExistsContactWithName(duplicated.Name, null)).Returns(true);

            var model = ContactsModelObjectMother.FromEntity(duplicated);

            var cmd = new AddContactCommand(model);
            var handler = new AddContactCommandHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<DomainException>(() => handler.Handle(cmd, new System.Threading.CancellationToken()));
        }
    }
}
