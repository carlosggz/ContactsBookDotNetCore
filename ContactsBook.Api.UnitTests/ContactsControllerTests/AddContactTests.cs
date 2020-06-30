using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class AddContactTests: BaseContactController
    {
        [Test]
        public void AddContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            model.Id = null;
            _contactsService.Setup(x => x.AddContact(model));

            var returned = _controller.Add(model);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is OkObjectResult);

            var ok = returned.Result as OkObjectResult;
            Assert.IsNotNull(ok.Value);

            var apiResult = ok.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.DoesNotThrow(() => new IdValueObject(apiResult.ContactId));
            Assert.IsNull(apiResult.Errors);
            Assert.IsTrue(apiResult.Success);

            _contactsService.VerifyAll();
        }

        [Test]
        public void NullModelThrowsException()
        {
            _logger.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            var returned = _controller.Add(null);

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

            _logger.VerifyAll();
        }

        [Test]
        public void InvalidModelThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            _contactsService.Setup(x => x.AddContact(model)).Throws(new InvalidEntityException("Invalid entity"));

            var returned = _controller.Add(model);

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
        public void InvalidModelValidationThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            var errors = new List<ValidationResult>();
            var maxErrors = new Random().Next(1, 10);

            for (var i = 0; i < maxErrors; i++)
                errors.Add(new ValidationResult(Faker.Lorem.Sentence()));

            _contactsService.Setup(x => x.AddContact(model)).Throws(new EntityValidationException(errors));

            var returned = _controller.Add(model);

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestObjectResult);

            var br = returned.Result as BadRequestObjectResult;
            Assert.IsNotNull(br.Value);

            var apiResult = br.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.IsNull(apiResult.ContactId);
            Assert.IsNotNull(apiResult.Errors);
            Assert.AreEqual(errors.Count, apiResult.Errors.Count());
            Assert.IsFalse(apiResult.Success);

            foreach (var error in apiResult.Errors)
                Assert.IsTrue(errors.Any(x => x.ErrorMessage == error));

            _contactsService.VerifyAll();
        }
    }
}
