using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsBook.Api.Models
{
    public class ContactsSearchCriteriaModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Text { get; set; }
    }
}
