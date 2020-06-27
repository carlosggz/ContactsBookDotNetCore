using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRepositories
{
    [TestFixture]
    public class ExistsContactWithNameTests : EFBaseTest
    {
        [Test]
        public void CheckReturnTrueForExistingContact()
        {
            var entity = ContactEntityObjectMother.Random();
            _repo.Add(entity);
            _context.SaveChanges();

            Assert.IsTrue(_repo.ExistsContactWithName(entity.Name));
            Assert.IsFalse(_repo.ExistsContactWithName(entity.Name, entity.Id));
        }

        [Test]
        public void CheckReturnFalseForNonExistingContact()
        {
            var entity = ContactEntityObjectMother.Random();

            Assert.IsFalse(_repo.ExistsContactWithName(entity.Name));
            Assert.IsFalse(_repo.ExistsContactWithName(entity.Name, entity.Id));
        }

        [Test]
        public void CheckReturnTrueForExistingContactWithOtherId()
        {
            var firstEntity = ContactEntityObjectMother.Random();
            _repo.Add(firstEntity);
            _context.SaveChanges();
            var secondEntity = ContactEntityObjectMother.Random();
            secondEntity.Name = firstEntity.Name;

            Assert.IsTrue(_repo.ExistsContactWithName(firstEntity.Name));
            Assert.IsFalse(_repo.ExistsContactWithName(firstEntity.Name, firstEntity.Id));
            Assert.IsTrue(_repo.ExistsContactWithName(secondEntity.Name, secondEntity.Id));
        }

        [Test]
        public void CheckWithNullEntityThrowsException()
        {
            Assert.Throws<InvalidParametersException>(() => _repo.ExistsContactWithName(null));
        }
    }
}
