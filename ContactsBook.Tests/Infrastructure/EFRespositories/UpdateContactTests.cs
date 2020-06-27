using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRespositories
{
    [TestFixture]
    public class UpdateContactTests: EFBaseTest
    {
        [Test]
        public void UpdateContactUpdateTheEntity()
        {
            var entity = ContactEntityObjectMother.Random();
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            _repo.Add(entity);
            _context.SaveChanges();
            entity.Name = ContactNameValueObjectObjectMother.Random();
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());

            _repo.Update(entity);

            _context.SaveChanges();
            VerifySaved(entity);
        }

        [Test]
        public void UpdateNullEntityThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _repo.Update(null));
        }

        [Test]
        public void UpdateNonExistingEntityThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _repo.Update(ContactEntityObjectMother.Random()));
        }
    }
}
