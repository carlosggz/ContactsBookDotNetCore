using ContactsBook.Api.Controllers;
using ContactsBook.Application.Services;
using ContactsBook.Common.Events;
using ContactsBook.Common.Logger;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using MediatR;
using Moq;
using NUnit.Framework;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    public abstract class BaseContactController
    {
        protected Mock<IMediator> mediator;
        protected Mock<IAppLogger> logger;
        protected Mock<IUnitOfWork> uow;
        protected Mock<IEventBus> eventBus;
        protected Mock<IContactsRepository> repository;
        protected ContactsController controller;

        [SetUp]
        public void Init()
        {
            logger = new Mock<IAppLogger>();
            mediator = new Mock<IMediator>();
            uow = new Mock<IUnitOfWork>();
            eventBus = new Mock<IEventBus>();
            repository = new Mock<IContactsRepository>();
            controller = new ContactsController(logger.Object, mediator.Object);
        }
    }
}
