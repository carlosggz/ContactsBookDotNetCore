using ContactsBook.Application.Dtos;
using ContactsBook.Tests.Common.ObjectMothers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;

namespace ContactsBook.Api.AcceptanceTests.Contacts
{
    [TestFixture]
    public class UpdateContactTests : BaseContactsTests
    {
        private void VerifyCall(ContactsModel model, System.Net.HttpStatusCode expectedCode)
        {
            var json = model == null ? string.Empty : JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = _client.PutAsync(ContactsApiUrl, content).Result;

            Assert.IsTrue(response.StatusCode == expectedCode);
        }

        [Test]
        public void ValidContactReturnsOk()
        {
            var entity = ContactEntityObjectMother.Random();

            uow.StartChanges();
            repository.Add(entity);
            uow.CommitChanges();

            var model = ContactsModelObjectMother.FromEntity(entity);

            VerifyCall(model, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public void ContactAlreadyExistsReturnBadRequest()
        {
            var firstContact = ContactEntityObjectMother.Random();
            var secondContact = ContactEntityObjectMother.Random();
            secondContact.Name = firstContact.Name;

            uow.StartChanges();
            repository.Add(firstContact);
            uow.CommitChanges();

            var model = ContactsModelObjectMother.FromEntity(secondContact);

            VerifyCall(model, System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public void InvalidContactReturnsBadRequest()
        {
            var model = ContactsModelObjectMother.Random();
            model.FirstName = null;

            VerifyCall(model, System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public void UnknownContactReturnsNotFound()
        {
            var model = ContactsModelObjectMother.Random();

            VerifyCall(model, System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public void NullReturnsBadRequest()
        {
            VerifyCall(null, System.Net.HttpStatusCode.BadRequest);
        }
    }
}