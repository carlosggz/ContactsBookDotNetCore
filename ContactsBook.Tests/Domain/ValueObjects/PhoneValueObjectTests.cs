using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.ObjectMothers;
using Faker;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class PhoneValueObjectTests
    {
        [Test]
        public void ValidPhoneValueGeneratesValidObject()
        {
            foreach (var phoneNumber in PhoneValueObjectObjectMother.PhoneNumbers)
            {
                var phoneType = PhoneValueObjectObjectMother.GetRandomPhoneType();
                var vo = new PhoneValueObject(phoneType, phoneNumber);

                Assert.NotNull(vo);
                Assert.AreEqual(phoneType, vo.PhoneType);
                Assert.AreEqual(phoneNumber, vo.PhoneNumber);
            }
        }

        [Test]
        public void ObjectsWithSameValuesAreEquals()
        {
            var random = PhoneValueObjectObjectMother.Random();

            var vo1 = new PhoneValueObject(random.PhoneType, random.PhoneNumber);
            var vo2 = new PhoneValueObject(random.PhoneType, random.PhoneNumber);

            Assert.NotNull(vo1);
            Assert.NotNull(vo2);
            Assert.AreEqual(vo1, vo2);
        }

        [Test]
        public void NullValueThrowsException()
            => Assert.Throws<DomainException>(() => new PhoneValueObject(PhoneValueObjectObjectMother.GetRandomPhoneType(), null));

        [Test]
        public void EmptyValueThrowsException()
            => Assert.Throws<DomainException>(() => new PhoneValueObject(PhoneValueObjectObjectMother.GetRandomPhoneType(), string.Empty));

        [Test]
        public void InvalidPhoneNumberThrowsException()
            => Assert.Throws<DomainException>(() => new PhoneValueObject(PhoneValueObjectObjectMother.GetRandomPhoneType(), "abc"));
    }
}
