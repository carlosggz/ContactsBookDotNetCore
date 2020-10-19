using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Services.GetContact;
using ContactsBook.Common.Exceptions;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class GetContactTests : BaseContactController
    {
        [Test]
        public void GetContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            mediator.Setup(x => x.Send(It.Is<GetContactQuery>(x => x.Id == model.Id), It.IsAny<CancellationToken>())).Returns(Task.FromResult(model));

            var returned = controller.Get(model.Id).Result;

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

            mediator.VerifyAll();
        }


        [Test]
        public void NullIdThrowsException()
        {
            mediator.Setup(x => x.Send(It.Is<GetContactQuery>(x => x.Id == null), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Get(null).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            mediator.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        {
            var id = Faker.Lorem.GetFirstWord();
            mediator.Setup(x => x.Send(It.Is<GetContactQuery>(x => x.Id == id), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Get(id).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            mediator.VerifyAll();
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            var id = ContactEntityObjectMother.Random().Id.Value;
            mediator.Setup(x => x.Send(It.Is<GetContactQuery>(x => x .Id == id), It.IsAny<CancellationToken>())).Throws(new EntityNotFound("Invalid entity"));

            var returned = controller.Get(id).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is NotFoundResult);

            mediator.VerifyAll();
        }
    }
}
