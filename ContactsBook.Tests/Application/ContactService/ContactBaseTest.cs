using ContactsBook.Application.Services;
using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
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
        protected Mock<IUnitOfWork> _uof;
        protected Mock<IEventBus> _eventBus;
        protected ContactsService _contactsService;

        [SetUp]
        public void Init()
        {
            _repo = new Mock<IContactsRepository>();
            _uof = new Mock<IUnitOfWork>();
            _eventBus = new Mock<IEventBus>();
            _contactsService = new ContactsService(_uof.Object, _eventBus.Object, _repo.Object);
        }

    }
}
