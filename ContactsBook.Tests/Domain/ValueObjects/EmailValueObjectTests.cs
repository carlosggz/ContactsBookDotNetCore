using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class EmailValueObjectTests
    {
        [Test]
        public void ValidEmailGeneratesValidObject()
        {
            var random = EmailValueObjectObjectMother.Random();

            var vo = new EmailValueObject(random.Value);

            Assert.NotNull(vo);
            Assert.AreEqual(random.Value, vo.Value);
        }

        [Test]
        public void ObjectsWithSameValueAreEquals()
        {
            var random = EmailValueObjectObjectMother.Random();

            var vo1 = new EmailValueObject(random.Value);
            var vo2 = new EmailValueObject(random.Value);

            Assert.NotNull(vo1);
            Assert.NotNull(vo2);
            Assert.AreEqual(vo1, vo2);
        }

        [Test]
        public void NullValueThrowsException()
            => Assert.Throws<DomainException>(() => new EmailValueObject(null));

        [Test]
        public void EmptyValueThrowsException()
            => Assert.Throws<DomainException>(() => new EmailValueObject(string.Empty));

        [Test]
        public void InvalidEmailThrowsException()
           => Assert.Throws<DomainException>(() => new EmailValueObject(DateTime.Now.ToString()));
    }
}
