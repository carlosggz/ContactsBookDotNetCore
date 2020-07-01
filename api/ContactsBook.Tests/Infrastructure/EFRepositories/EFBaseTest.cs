using ContactsBook.Domain.Contacts;
using ContactsBook.Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Tests.Infrastructure.EFRepositories
{
    public class EFBaseTest
    {
        private readonly DbContextOptions<ContactsBookContext> _options = new DbContextOptionsBuilder<ContactsBookContext>()
                       .UseInMemoryDatabase(databaseName: "Test")
                       .Options;
        protected ContactsBookContext _context;
        protected ContactsRepository _repo;

        [SetUp]
        public void Init()
        {
            _context = new ContactsBookContext(_options);
            _repo = new ContactsRepository(_context);
        }

        [TearDown]
        public void Clear()
        {
            foreach (var contact in _context.Contacts)
                _context.Contacts.Remove(contact);

            _context.SaveChanges();
            _context.Dispose();
        }

        protected void VerifySaved(ContactEntity entity)
        {
            var model = _context.Contacts.SingleOrDefault(x => x.Id == entity.Id.Value);
            Assert.IsNotNull(model);
            Assert.AreEqual(entity.Name.FirstName, model.FirstName);
            Assert.AreEqual(entity.Name.LastName, model.LastName);

            var emails = _context.ContactEmails.Where(x => x.ContactId == entity.Id.Value).Select(x => x.Email).ToList();
            Assert.AreEqual(entity.EmailAddresses.Count, emails.Count);

            foreach (var email in emails)
                Assert.IsTrue(entity.EmailAddresses.Contains(new EmailValueObject(email)));

            var phones = _context.ContactPhones.Where(x => x.ContactId == entity.Id.Value).ToList();
            Assert.AreEqual(entity.PhoneNumbers.Count, phones.Count);

            foreach (var phone in phones)
                Assert.IsTrue(entity.PhoneNumbers.Contains(new PhoneValueObject(phone.PhoneType, phone.PhoneNumber)));
        }
    }
}
