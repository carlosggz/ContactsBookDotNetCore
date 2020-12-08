using ContactsBook.Api.Models;
using ContactsBook.Tests.Common.ObjectMothers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace ContactsBook.Api.AcceptanceTests.Contacts
{
    [TestFixture]
    public class SearchContactTests : BaseContactsTests
    {
        private void VerifyCall(ContactsSearchCriteriaModel criteria, System.Net.HttpStatusCode expectedCode)
        {
            var json = criteria == null ? string.Empty : JsonConvert.SerializeObject(criteria);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = _client.PostAsync(ContactsApiUrl + "/search", content).Result;

            Assert.IsTrue(response.StatusCode == expectedCode);
        }

        [Test]
        public void ValidSearchReturnsOk()
        {
            VerifyCall(new ContactsSearchCriteriaModel() { PageNumber = 1, PageSize = 1 }, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public void InvalidParametersReturnBadRequest()
        {
            VerifyCall(new ContactsSearchCriteriaModel() { PageNumber = 0, PageSize = 1 }, System.Net.HttpStatusCode.BadRequest);
            VerifyCall(new ContactsSearchCriteriaModel() { PageNumber = 1, PageSize = 0 }, System.Net.HttpStatusCode.BadRequest);
            VerifyCall(null, System.Net.HttpStatusCode.BadRequest);
        }
    }
}