using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Services.GetContacts;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NLog.LayoutRenderers.Wrappers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsBook.Api.UnitTests.ContactsControllerTests
{
    [TestFixture]
    public class SearchContactsTests : BaseContactController
    {
        [Test]
        public void SearchContactCallsCollaborators()
        {
            var model = ContactsModelObjectMother.Random();
            
            var dto = new ContactDto()
            {
                ContactId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailsCount = model.EmailAddresses.Count(),
                PhonesCount = model.PhoneNumbers.Count()
            };

            var result = new SearchResults<ContactDto>(1, new List<ContactDto>() { dto });
            mediator.Setup(x => x.Send(It.IsAny<GetContactsQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(result));

            var returned = controller.Search(new ContactsSearchCriteriaModel()).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is OkObjectResult);

            var ok = returned.Result as OkObjectResult;
            Assert.IsNotNull(ok.Value);

            var apiResult = ok.Value as SearchResults<ContactDto>;
            Assert.IsNotNull(apiResult);

            Assert.AreEqual(result.Total, apiResult.Total);
            Assert.IsNotNull(apiResult.Results);
            Assert.AreEqual(result.Results.Count(), apiResult.Results.Count());

            foreach (var contactDto in apiResult.Results)
                Assert.IsTrue(result.Results.Any(x => x.ContactId == contactDto.ContactId));

            mediator.VerifyAll();
        }


        [Test]
        public void NullParametersThrowsException()
        {
            var returned = controller.Search(null).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);
        }

        [Test]
        public void InvalidPageNumberParameterThrowsException()
        {
            var criteria = new ContactsSearchCriteriaModel() { PageNumber = 0, PageSize = 1 };
            mediator
                .Setup(x => x.Send(It.Is<GetContactsQuery>(x => x.Page == criteria.PageNumber && x.Size == criteria.PageSize && x.Text == criteria.Text), It.IsAny<CancellationToken>()))
                .Throws(new InvalidParametersException("Invalid search parameters"));

            var returned = controller.Search(criteria).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            mediator.VerifyAll();
        }

        [Test]
        public void InvalidPageSizeParameterThrowsException()
        {
            var criteria = new ContactsSearchCriteriaModel() { PageNumber = 1, PageSize = 0 };
            mediator
                .Setup(x => x.Send(It.Is<GetContactsQuery>(x => x.Page == criteria.PageNumber && x.Size == criteria.PageSize && x.Text == criteria.Text), It.IsAny<CancellationToken>()))
                .Throws(new InvalidParametersException("Invalid search parameters"));

            var returned = controller.Search(criteria).Result;

            Assert.IsNotNull(returned);
            Assert.IsNotNull(returned.Result);
            Assert.IsTrue(returned.Result is BadRequestResult);

            mediator.VerifyAll();
        }

    }
}
