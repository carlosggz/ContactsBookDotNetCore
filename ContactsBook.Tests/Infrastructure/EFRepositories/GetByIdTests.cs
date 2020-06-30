using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRepositories
{
    [TestFixture]
    public class GetByIdTests : EFBaseTest
    {
        [Test]
        public void GetAnUnknowContactReturnNull()
        {
            Assert.IsNull(_repo.GetById(new IdValueObject()));
        }

        [Test]
        public void GetWithNullEntityThrowsException()
        {
            Assert.Throws<InvalidParametersException>(() => _repo.GetById(null));
        }

        [Test]
        public void GetAnExistingContactReturnEntity()
        {
            var entity = ContactEntityObjectMother.Random();
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            _repo.Add(entity);
            _context.SaveChanges();

            var fromRepo = _repo.GetById(entity.Id);

            Assert.IsNotNull(fromRepo);
            Assert.AreEqual(entity.Id, fromRepo.Id);
            Assert.AreEqual(entity.Name, fromRepo.Name);
            Assert.AreEqual(entity.EmailAddresses.Count, fromRepo.EmailAddresses.Count);
            Assert.AreEqual(entity.PhoneNumbers.Count, fromRepo.PhoneNumbers.Count);

            foreach (var email in entity.EmailAddresses)
                Assert.IsTrue(fromRepo.EmailAddresses.Contains(email));

            foreach (var phone in entity.PhoneNumbers)
                Assert.IsTrue(fromRepo.PhoneNumbers.Contains(phone));
        }
    }
}
