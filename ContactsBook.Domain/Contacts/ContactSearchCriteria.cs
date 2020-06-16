using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class ContactSearchCriteria : BaseSearchCriteria
    {
        private readonly string _text;

        public ContactSearchCriteria(int pageNumber, int pageSize, string text)
            :base(pageNumber, pageSize)
        {
            _text = text;
        }
    }
}
