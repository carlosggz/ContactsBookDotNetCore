using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class ContactSearchCriteria : BaseSearchCriteria
    {
        public readonly string Text;

        public ContactSearchCriteria(
            int pageNumber, 
            int pageSize, 
            string text = null
            )
            :base(pageNumber, pageSize)
        {
            Text = text;
        }
    }
}
