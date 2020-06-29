using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsBook.Api.Models
{
    public class ApiContactResultModel
    {
        public string ContactId { get; set; } = null;
        public bool Success { get; set; } = false;

        public IEnumerable<string> Errors { get; set; } = null;
    }
}
