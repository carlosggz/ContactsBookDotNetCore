using ContactsBook.Tests.Common.ObjectMothers;
using NUnit.Framework;
using System;

namespace ContactsBook.Api.AcceptanceTests.Contacts
{
    [TestFixture]
    public class DeleteContactTests : BaseContactsTests
    {
        private void VerifyCall(string url, System.Net.HttpStatusCode expectedCode)
        {
            var response = _client.DeleteAsync(ContactsApiUrl + url).Result;

            Assert.IsTrue(response.StatusCode == expectedCode);
        }

        [Test]
        public void ValidContactReturnsOk()
        {
            var entity = ContactEntityObjectMother.Random();
            uow.StartChanges();
            repository.Add(entity);
            uow.CommitChanges();

            VerifyCall("/" + entity.Id.Value, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public void InvalidContactReturnsNotFound()
        {
            var entity = ContactEntityObjectMother.Random();

            VerifyCall("/" + entity.Id.Value, System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public void InvalidIdReturnsBadRequest()
        {
            VerifyCall("/" + new Random().Next(1, 100), System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public void NullIdReturnsNotfound()
        {
            VerifyCall(string.Empty, System.Net.HttpStatusCode.MethodNotAllowed);
        }
    }
}