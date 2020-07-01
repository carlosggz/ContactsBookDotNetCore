using ContactsBook.Common.Exceptions;
using ContactsBook.Domain.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class IdValueObjectTests
    {
        [Test]
        public void ValidIdGeneratesValidObject()
        {
            var id = new IdValueObject();

            var vo = new IdValueObject(id.Value);

            Assert.NotNull(vo);
            Assert.AreEqual(id.Value, vo.Value);
        }

        [Test]
        public void ObjectsWithSameValueAreEquals()
        {
            var id = new IdValueObject();

            var vo1 = new IdValueObject(id.Value);
            var vo2 = new IdValueObject(id.Value);

            Assert.NotNull(vo1);
            Assert.NotNull(vo2);
            Assert.AreEqual(vo1, vo2);
        }

        [Test]
        public void NullValueThrowsException()
            => Assert.Throws<DomainException>(() => new IdValueObject(null));

        [Test]
        public void EmptyValueThrowsException()
            => Assert.Throws<DomainException>(() => new IdValueObject(string.Empty));

        [Test]
        public void InvalidEmailThrowsException()
            => Assert.Throws<DomainException>(() => new IdValueObject(DateTime.Now.ToString()));
    }
}
