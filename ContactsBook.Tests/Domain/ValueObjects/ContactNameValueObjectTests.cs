using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class ContactNameValueObjectTests
    {
        [Test]
        public void ValidPhoneValueGeneratesValidObject()
        {
            var firstName = Faker.Name.First();
            var lastName = Faker.Name.Last();

            var vo = new ContactNameValueObject(firstName, lastName);

            Assert.NotNull(vo);
            Assert.AreEqual(firstName, vo.FirstName);
            Assert.AreEqual(lastName, vo.LastName);
        }

        [Test]
        public void ObjectsWithSameValuesAreEquals()
        {
            var firstName = Faker.Name.First();
            var lastName = Faker.Name.Last();

            var vo1 = new ContactNameValueObject(firstName, lastName);
            var vo2 = new ContactNameValueObject(firstName, lastName);

            Assert.NotNull(vo1);
            Assert.NotNull(vo2);
            Assert.AreEqual(vo1, vo2);
        }

        [Test]
        public void NullValueThrowsException()
        {
            Assert.Throws<DomainException>(() => new ContactNameValueObject(null, null));
            Assert.Throws<DomainException>(() => new ContactNameValueObject(Faker.Name.First(), null));
            Assert.Throws<DomainException>(() => new ContactNameValueObject(null, Faker.Name.Last()));            
        }

        [Test]
        public void EmptyValueThrowsException()
        {
            Assert.Throws<DomainException>(() => new ContactNameValueObject(string.Empty, string.Empty));
            Assert.Throws<DomainException>(() => new ContactNameValueObject(Faker.Name.First(), string.Empty));
            Assert.Throws<DomainException>(() => new ContactNameValueObject(string.Empty, Faker.Name.Last()));
        }
    }

    public static class ObjectMotherContactNameValueObject
    { 
    
    }
}
