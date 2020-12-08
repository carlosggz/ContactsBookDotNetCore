using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ContactsBook.Api.AcceptanceTests.Contacts
{
    [TestFixture]
    public class GetContactTests : BaseContactsTests
    {
        private void VerifyCall(string url, System.Net.HttpStatusCode expectedCode)
        {
            var response = _client.GetAsync(ContactsApiUrl + url).Result;

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
        public void NullIdReturnsNotAllowed()
        {
            VerifyCall(string.Empty, System.Net.HttpStatusCode.MethodNotAllowed);
        }
    }
}