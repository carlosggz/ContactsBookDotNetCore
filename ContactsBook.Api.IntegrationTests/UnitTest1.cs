using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;

namespace ContactsBook.Api.IntegrationTests
{
    [TestFixture]
    public class Tests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public Tests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<ContactsBook.Api.Startup>());
            _client = _server.CreateClient();
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}