using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services.UpdateContact;
using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Tests.Common.ObjectMothers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class UpdateContactTests : BaseContactController
    {
        [Test]
        public void UpdateContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            mediator.Setup(x => x.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var returned = controller.Update(model).Result;

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
            mediator.Setup(x => x.Send(It.Is<UpdateContactCommand>(x => x.Model == null), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Update(null).Result;

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
        public void InvalidModelThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            mediator.Setup(x => x.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<CancellationToken>())).Throws(new InvalidEntityException("Invalid entity"));

            var returned = controller.Update(model).Result;

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

            mediator.Setup(x => x.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<CancellationToken>())).Throws(new EntityValidationException(errors));

            var returned = controller.Update(model).Result;

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

        [Test]
        public void InvalidContactThrowsException()
        {
            var model = ContactsModelObjectMother.Random();
            var errors = new List<ValidationResult>();
            var maxErrors = new Random().Next(1, 10);

            for (var i = 0; i < maxErrors; i++)
                errors.Add(new ValidationResult(Faker.Lorem.Sentence()));

            mediator.Setup(x => x.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<CancellationToken>())).Throws(new EntityNotFound("Invalid contact"));

            var returned = controller.Update(model).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is NotFoundObjectResult);

            var notfound = returned.Result as NotFoundObjectResult;
            Assert.IsNotNull(notfound.Value);

            var apiResult = notfound.Value as ApiContactResultModel;
            Assert.IsNotNull(apiResult);

            Assert.IsNull(apiResult.ContactId);
            Assert.IsNotNull(apiResult.Errors);
            Assert.AreEqual(1, apiResult.Errors.Count());
            Assert.IsFalse(apiResult.Success);

            mediator.VerifyAll();
        }
    }
    }
