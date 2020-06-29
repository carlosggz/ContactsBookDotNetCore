using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContactsBook.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
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
