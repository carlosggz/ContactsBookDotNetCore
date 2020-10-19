using ContactsBook.Api.Models;
using ContactsBook.Application.Services.DeleteContact;
using ContactsBook.Common.Exceptions;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class DeleteContactTests : BaseContactController
    {
        [Test]
        public void DeleteContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            mediator.Setup(x => x.Send(It.Is<DeleteContactCommand>(x => x.Id == model.Id), It.IsAny<CancellationToken>()));

            var returned = controller.Delete(model.Id).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is OkObjectResult);

            var ok = returned.Result as OkObjectResult;
            Assert.IsNotNull(ok.Value);

            var apiResult = ok.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.AreEqual(model.Id, apiResult.ContactId);
            Assert.IsNull(apiResult.Errors);
            Assert.IsTrue(apiResult.Success);

            mediator.VerifyAll();
        }


        [Test]
        public void NullModelThrowsException()
        {
            mediator.Setup(x => x.Send(It.Is<DeleteContactCommand>(x => x.Id == null), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));
            
            var returned = controller.Delete(null).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestObjectResult);

            var br = returned.Result as BadRequestObjectResult;
            Assert.IsNotNull(br.Value);

            var apiResult = br.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.IsNull(apiResult.ContactId);
            Assert.IsNotNull(apiResult.Errors);
            Assert.AreEqual(1, apiResult.Errors.Count());
            Assert.IsFalse(apiResult.Success);

            mediator.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        {
            var id = Faker.Lorem.GetFirstWord();
            mediator.Setup(x => x.Send(It.Is<DeleteContactCommand>(x => x.Id == id), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Delete(id).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestObjectResult);

            var br = returned.Result as BadRequestObjectResult;
            Assert.IsNotNull(br.Value);

            var apiResult = br.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.IsNull(apiResult.ContactId);
            Assert.IsNotNull(apiResult.Errors);
            Assert.AreEqual(1, apiResult.Errors.Count());
            Assert.IsFalse(apiResult.Success);

            mediator.VerifyAll();
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            var id = ContactEntityObjectMother.Random().Id.Value;
            mediator.Setup(x => x.Send(It.Is<DeleteContactCommand>(x => x.Id == id), It.IsAny<CancellationToken>())).Throws(new EntityNotFound("Invalid entity"));

            var returned = controller.Delete(id).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is NotFoundObjectResult);

            var br = returned.Result as NotFoundObjectResult;
            Assert.IsNotNull(br.Value);

            var apiResult = br.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.IsNull(apiResult.ContactId);
            Assert.IsNotNull(apiResult.Errors);
            Assert.AreEqual(1, apiResult.Errors.Count());
            Assert.IsFalse(apiResult.Success);

            mediator.VerifyAll();
        }
    }
}
