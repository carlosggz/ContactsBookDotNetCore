using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRepositories
{
    [TestFixture]
    public class SearchByCriteriaTests : EFBaseTest
    {
        [Test]
        public void SearchWithoutContactsReturnEmptyresult()
        {
            var results = _repo.SearchByCriteria(new ContactSearchCriteria(1, 1));

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Total);
            Assert.IsNotNull(results.Results);
            Assert.AreEqual(0, results.Results.Count());
        }

        [Test]
        public void SearchWithoutMatchesReturnEmptyresult()
        {
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _context.SaveChanges();

            var results = _repo.SearchByCriteria(new ContactSearchCriteria(1, 1, DateTime.Now.ToLongDateString() + DateTime.Now.Ticks.ToString()));

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Total);
            Assert.IsNotNull(results.Results);
            Assert.AreEqual(0, results.Results.Count());
        }

        [Test]
        public void SearchWithMatchesReturnCorrectResult()
        {
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            _repo.Add(ContactEntityObjectMother.Random());
            var first = ContactEntityObjectMother.Random();
            first.Name = new ContactNameValueObject("Peter", "Parker");
            var second = ContactEntityObjectMother.Random();
            second.Name = new ContactNameValueObject("Peter", "Cloud");
            var third = ContactEntityObjectMother.Random();
            third.Name = new ContactNameValueObject("Foo", "Pet");
            _repo.Add(first);
            _repo.Add(second);
            _repo.Add(third);
            _context.SaveChanges();

            var results1 = _repo.SearchByCriteria(new ContactSearchCriteria(1, 1, "Pet"));
            var results2 = _repo.SearchByCriteria(new ContactSearchCriteria(2, 1, "Pet"));
            var results3 = _repo.SearchByCriteria(new ContactSearchCriteria(3, 1, "Pet"));
            var results4 = _repo.SearchByCriteria(new ContactSearchCriteria(1, 3, "Pet"));

            Assert.IsNotNull(results1);
            Assert.IsNotNull(results2);
            Assert.IsNotNull(results3);
            Assert.IsNotNull(results4);
            Assert.AreEqual(3, results1.Total);
            Assert.AreEqual(3, results2.Total);
            Assert.AreEqual(3, results3.Total);
            Assert.AreEqual(3, results4.Total);
            Assert.IsNotNull(results1.Results);
            Assert.IsNotNull(results2.Results);
            Assert.IsNotNull(results3.Results);
            Assert.IsNotNull(results4.Results);
            Assert.AreEqual(1, results1.Results.Count());
            Assert.AreEqual(1, results2.Results.Count());
            Assert.AreEqual(1, results3.Results.Count());
            Assert.AreEqual(3, results4.Results.Count());
            Assert.AreEqual(first.Id.Value, results1.Results.First().ContactId);
            Assert.AreEqual(second.Id.Value, results2.Results.First().ContactId);
            Assert.AreEqual(third.Id.Value, results3.Results.First().ContactId);
            var r4 = results4.Results.ToList();
            Assert.AreEqual(first.Id.Value, r4[0].ContactId);
            Assert.AreEqual(second.Id.Value, r4[1].ContactId);
            Assert.AreEqual(third.Id.Value, r4[2].ContactId);
        }
    }
}
