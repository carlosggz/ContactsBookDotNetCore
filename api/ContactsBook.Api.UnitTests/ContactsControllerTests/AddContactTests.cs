using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services.AddContact;
using ContactsBook.Common.Events;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class AddContactTests: BaseContactController
    {
        [Test]
        public void AddContactCallsCollaboratorsAsync()
        {
            var model = ContactsModelObjectMother.Random();
            model.Id = null;
            mediator.Setup(x => x.Send(It.IsAny<AddContactCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var returned = controller.Add(model).Result;

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

            mediator.VerifyAll();
        }

        [Test]
        public void NullModelThrowsException()
        {
            logger.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            var returned = controller.Add(null).Result;

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

            logger.VerifyAll();
        }

        [Test]
        public void InvalidModelThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            mediator.Setup(x => x.Send(It.IsAny<AddContactCommand>(), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Add(model).Result;

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
        public void InvalidModelValidationThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            var errors = new List<ValidationResult>();
            var maxErrors = new Random().Next(1, 10);

            for (var i = 0; i < maxErrors; i++)
                errors.Add(new ValidationResult(Faker.Lorem.Sentence()));

            mediator.Setup(x => x.Send(It.IsAny<AddContactCommand>(), It.IsAny<CancellationToken>())).Throws(new EntityValidationException(errors));

            var returned = controller.Add(model).Result;

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

            mediator.VerifyAll();
        }
    }
}
