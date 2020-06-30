using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
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

            _repo.Setup(
                x => x.SearchByCriteria(It.Is<ContactSearchCriteria>(p => p.PageNumber == page && p.PageSize == size && p.Text == text)))
                .Returns(toReturn);

            var result = _contactsService.GetContactsByName(page, size, text);

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
            Assert.Throws<InvalidParametersException>(() => _contactsService.GetContactsByName(0, 1, string.Empty));
            Assert.Throws<InvalidParametersException>(() => _contactsService.GetContactsByName(1, 0, string.Empty));
            Assert.DoesNotThrow(() => _contactsService.GetContactsByName(1, 1, string.Empty));
        }
    }
}
