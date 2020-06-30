using ContactsBook.Api.Models;
using ContactsBook.Common.Exceptions;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class DeleteContactTests : BaseContactController
    {
        [Test]
        public void DeleteContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            _contactsService.Setup(x => x.DeleteContact(model.Id));

            var returned = _controller.Delete(model.Id);

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

            _contactsService.VerifyAll();
        }


        [Test]
        public void NullModelThrowsException()
        {
            _contactsService.Setup(x => x.DeleteContact(null)).Throws(new InvalidEntityException("Invalid entity"));

            var returned = _controller.Delete(null);

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

            _contactsService.VerifyAll();
        }

        [Test]
        public void InvalidIdThrowsException()
        {
            var id = Faker.Lorem.GetFirstWord();
            _contactsService.Setup(x => x.DeleteContact(id)).Throws(new InvalidEntityException("Invalid entity"));

            var returned = _controller.Delete(id);

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

            _contactsService.VerifyAll();
        }

        [Test]
        public void InvalidContactThrowsException()
        {
            var id = ContactEntityObjectMother.Random().Id.Value;
            _contactsService.Setup(x => x.DeleteContact(id)).Throws(new EntityNotFound("Invalid entity"));

            var returned = _controller.Delete(id);

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

            _contactsService.VerifyAll();
        }
    }
}
