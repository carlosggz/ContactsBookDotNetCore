using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRepositories
{
    [TestFixture]
    public class AddContactTests : EFBaseTest
    {
        [Test]
        public void AddContactAddTheEntity()
        {
            var entity = ContactEntityObjectMother.Random();
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());

            _repo.Add(entity);

            _context.SaveChanges();
            VerifySaved(entity);
        }

        [Test]
        public void AddNullEntityThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _repo.Add(null));
        }
    }
}
