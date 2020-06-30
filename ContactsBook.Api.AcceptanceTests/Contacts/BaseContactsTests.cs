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
using System.Threading.Tasks;

namespace ContactsBook.Api.AcceptanceTests.Contacts
{
    public abstract class BaseContactsTests
    {
        protected const string ContactsApiUrl = "/api/Contacts";
        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        protected IContactsBookUnitOfWork _uow;

        public BaseContactsTests()
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(appConfig)
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            _server = new TestServer(hostBuilder);
            _client = _server.CreateClient();
        }

        [SetUp]
        public void Setup()
        {
            _uow = _server.Host.Services.GetService<IContactsBookUnitOfWork>();
        }
    }
}
