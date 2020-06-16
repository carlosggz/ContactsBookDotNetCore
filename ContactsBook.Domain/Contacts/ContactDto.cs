using ContactsBook.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class ContactDto: IDto
    {
        public string ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmailsCount { get; set; }
        public int PhonesCount { get; set; }
    }
}
