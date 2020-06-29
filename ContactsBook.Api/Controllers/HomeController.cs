using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsBook.Application.Services;
using ContactsBook.Common.Logger;
using ContactsBook.Common.Mailer;
using Microsoft.AspNetCore.Mvc;

namespace ContactsBook.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IAppLogger logger;
        private readonly IMailer mailer;
        private readonly IContactsAppService service;

        public HomeController(IAppLogger logger, IMailer mailer, IContactsAppService service)
        {
            this.logger = logger;
            this.mailer = mailer;
            this.service = service;
        }
        [HttpGet]
        public string GetSalutation()
        {
            return "Hello";
        }

        [HttpGet]
        public DateTime GetDate()
        {
            return DateTime.Now; ;
        }

        [HttpGet]
        [ActionName("get-current-day")]
        public int GetDay()
        {
            return DateTime.Now.Day;
        }
    }
}
