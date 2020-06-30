using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Common.Exceptions;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class GetContactTests : BaseContactController
    {
        [Test]
        public void GetContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            _contactsService.Setup(x => x.GetContact(model.Id)).Returns(model);

            var returned = _controller.Get(model.Id);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is OkObjectResult);

            var ok = returned.Result as OkObjectResult;
            Assert.IsNotNull(ok.Value);

            var apiResult = ok.Value as ContactsModel;
            Assert.IsNotNull(apiResult);

            Assert.AreEqual(model.Id, apiResult.Id);
            Assert.AreEqual(model.FirstName, apiResult.FirstName);
            Assert.AreEqual(model.LastName, apiResult.LastName);
            Assert.IsNotNull(apiResult.PhoneNumbers);
            Assert.IsNotNull(apiResult.EmailAddresses);
            Assert.AreEqual(model.PhoneNumbers.Count(), apiResult.PhoneNumbers.Count());
            Assert.AreEqual(model.EmailAddresses.Count(), apiResult.EmailAddresses.Count());

            foreach (var email in apiResult.EmailAddresses)
                Assert.IsTrue(model.EmailAddresses.Any(x => x == email));

            foreach (var phone in apiResult.PhoneNumbers)
                Assert.IsTrue(model.PhoneNumbers.Any(x => x.PhoneType == phone.PhoneType && x.PhoneNumber == phone.PhoneNumber));

            _contactsService.VerifyAll();
        }


        [Test]
        public void NullIdThrowsException()
        {
            _contactsService.Setup(x => x.GetContact(null)).Throws(new InvalidEntityException("Invalid entity"));

            var returned = _controller.Get(null);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            _contactsService.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        {
            var id = Faker.Lorem.GetFirstWord();
            _contactsService.Setup(x => x.GetContact(id)).Throws(new InvalidEntityException("Invalid entity"));

            var returned = _controller.Get(id);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            _contactsService.VerifyAll();
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            var id = ContactEntityObjectMother.Random().Id.Value;
            _contactsService.Setup(x => x.GetContact(id)).Throws(new EntityNotFound("Invalid entity"));

            var returned = _controller.Get(id);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is NotFoundResult);

            _contactsService.VerifyAll();
        }
    }
}
