using ContactsBook.Application.Services;
using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Application.ContactService
{
    public abstract class ContactBaseTest
    {
        protected Mock<IContactsRepository> _repo;
        protected Mock<IContactsBookUnitOfWork> _uof;
        protected Mock<IEventBus> _eventBus;
        protected ContactsAppService _contactsService;

        [SetUp]
        public void Init()
        {
            _repo = new Mock<IContactsRepository>();
            _uof = new Mock<IContactsBookUnitOfWork>();
            _eventBus = new Mock<IEventBus>();

            _uof.Setup(x => x.ContactsRepository).Returns(_repo.Object);

            _contactsService = new ContactsAppService(_uof.Object, _eventBus.Object);
        }

    }
}
