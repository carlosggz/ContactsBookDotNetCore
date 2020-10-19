using ContactsBook.Application.Services.GetContacts;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactsBook.Tests.Application.ContactService
{
    [TestFixture]
    public class SearchContactsTest: ContactBaseTest
    {
        [Test]
        public void SearchCallsCollaborators()
        {
            var rnd = new Random();
            var page = rnd.Next(1, 100);
            var size = rnd.Next(1, 100);
            var text = Faker.Lorem.GetFirstWord();
            var contact = ContactEntityObjectMother.Random();
            var toReturn = new SearchResults<ContactDto>(
                rnd.Next(1, 100), 
                new List<ContactDto>() 
                    { 
                        new ContactDto() 
                        { 
                            FirstName = contact.Name.FirstName, 
                            LastName = contact.Name.LastName, 
                            ContactId = contact.Id.Value, 
                            EmailsCount = contact.EmailAddresses.Count,
                            PhonesCount = contact.PhoneNumbers.Count
                        } 
                    }
            );

            repo.Setup(
                x => x.SearchByCriteria(It.Is<ContactSearchCriteria>(p => p.PageNumber == page && p.PageSize == size && p.Text == text)))
                .Returns(toReturn);

            var cmd = new GetContactsQuery(page, size, text);
            var handler = new GetContactsQueryHandler(uow.Object, eventBus.Object, repo.Object);

            var result = handler.Handle(cmd, new System.Threading.CancellationToken()).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(toReturn.Total, result.Total);
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual(contact.Id.Value, result.Results.First().ContactId);
            Assert.AreEqual(contact.Name.FirstName, result.Results.First().FirstName);
            Assert.AreEqual(contact.Name.LastName, result.Results.First().LastName);
            Assert.AreEqual(contact.EmailAddresses.Count, result.Results.First().EmailsCount);
            Assert.AreEqual(contact.PhoneNumbers.Count, result.Results.First().PhonesCount);
        }

        [Test]
        public void SearchWithInvalidParametersThrowsException()
        {
            var handler = new GetContactsQueryHandler(uow.Object, eventBus.Object, repo.Object);

            Assert.Throws<InvalidParametersException>(() => handler.Handle(new GetContactsQuery(0, 1, string.Empty), new System.Threading.CancellationToken()));
            Assert.Throws<InvalidParametersException>(() => handler.Handle(new GetContactsQuery(1, 0, string.Empty), new System.Threading.CancellationToken()));
            Assert.DoesNotThrow(() => handler.Handle(new GetContactsQuery(1, 1, string.Empty), new System.Threading.CancellationToken()));
        }
    }
}
