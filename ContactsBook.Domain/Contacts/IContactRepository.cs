using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain.Contacts
{
    public interface IContactRepository: IRepository<Contact, IdValueObject, ContactSearchCriteria, ContactDto>
    {
    }
}
