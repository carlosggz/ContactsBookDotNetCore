using ContactsBook.Api.Controllers;
using ContactsBook.Application.Services;
using ContactsBook.Common.Logger;
using Moq;
using NUnit.Framework;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    public abstract class BaseContactController
    {
        protected Mock<IContactsAppService> _contactsService;
        protected Mock<IAppLogger> _logger;
        protected ContactsController _controller;

        [SetUp]
        public void Init()
        {
            _logger = new Mock<IAppLogger>();
            _contactsService = new Mock<IContactsAppService>();
            _controller = new ContactsController(_logger.Object, _contactsService.Object);
        }
    }
}
