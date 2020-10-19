using ContactsBook.Application.Services;
using ContactsBook.Common.Events;
using ContactsBook.Common.Logger;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Application.ContactService
{
    public abstract class ContactBaseTest
    {
        protected Mock<IContactsRepository> repo;
        protected Mock<IUnitOfWork> uow;
        protected Mock<IEventBus> eventBus;
        protected Mock<IMediator> mediator;
        protected Mock<IAppLogger> logger;

        [SetUp]
        public void Init()
        {
            repo = new Mock<IContactsRepository>();
            uow = new Mock<IUnitOfWork>();
            eventBus = new Mock<IEventBus>();
            logger = new Mock<IAppLogger>();
            mediator = new Mock<IMediator>();
            eventBus = new Mock<IEventBus>();

        }

    }
}
