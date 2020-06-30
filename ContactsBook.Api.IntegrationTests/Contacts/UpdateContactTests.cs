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
        private const string Url = ContactsApiUrl + "/Update";

        private void VerifyCall(ContactsModel model, System.Net.HttpStatusCode expectedCode)
        {
            var json = model == null ? string.Empty : JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = _client.PutAsync(Url, content).Result;

            Assert.IsTrue(response.StatusCode == expectedCode);
        }

        [Test]
        public void ValidContactReturnsOk()
        {
            var entity = ContactEntityObjectMother.Random();

            _uow.StartChanges();
            _uow.ContactsRepository.Add(entity);
            _uow.CommitChanges();

            var model = ContactsModelObjectMother.FromEntity(entity);

            VerifyCall(model, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public void ContactAlreadyExistsReturnBadRequest()
        {
            var firstContact = ContactEntityObjectMother.Random();
            var secondContact = ContactEntityObjectMother.Random();
            secondContact.Name = firstContact.Name;

            _uow.StartChanges();
            _uow.ContactsRepository.Add(firstContact);
            _uow.CommitChanges();

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