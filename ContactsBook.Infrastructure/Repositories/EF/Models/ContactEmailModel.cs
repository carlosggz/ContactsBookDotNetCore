using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF.Models
{
    public class ContactEmailModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string ContactId { get; set; }
        public ContactModel Contact { get; set; }
    }
}
