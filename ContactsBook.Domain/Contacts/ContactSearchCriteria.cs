using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public class ContactSearchCriteria : BaseSearchCriteria
    {
        private readonly string _containingText;

        public ContactSearchCriteria(
            int pageNumber, 
            int pageSize, 
            string containingText = null
            )
            :base(pageNumber, pageSize)
        {
            _containingText = containingText;
        }
    }
}
