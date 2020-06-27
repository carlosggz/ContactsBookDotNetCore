using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF.Models
{
    public class ContactPhoneModel
    {
        public int Id { get; set; }
        public PhoneType PhoneType { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactId { get; set; }
        public ContactModel Contact { get; set; }
    }
}
