using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using ContactsBook.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRespositories
{
    [TestFixture]
    public class DeleteContactTests: EFBaseTest
    {
        [Test]
        public void DeleteRemoveTheContact()
        {
            var entity = ContactEntityObjectMother.Random();
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddEmailAddress(EmailValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            entity.AddPhoneNumber(PhoneValueObjectObjectMother.Random());
            _repo.Add(entity);
            _context.SaveChanges();

            _repo.Delete(entity.Id);

            _context.SaveChanges();
            Assert.IsFalse(_context.Contacts.Any(x => x.Id == entity.Id.Value));
            Assert.IsFalse(_context.ContactEmails.Any(x => x.ContactId == entity.Id.Value));
            Assert.IsFalse(_context.ContactPhones.Any(x => x.ContactId == entity.Id.Value));
        }

        [Test]
        public void DeleteWithInvalidIdThrowsException()
        {
            Assert.Throws<InvalidParametersException>(() => _repo.Delete(null));
        }

        [Test]
        public void DeleteUnknownContactThrowsException()
        {
            Assert.Throws<InvalidEntityException>(() => _repo.Delete(new IdValueObject()));
        }
    }
}
