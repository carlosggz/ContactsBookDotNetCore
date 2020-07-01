using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Domain
{
    //Unit of work and repositories factory
    public interface IContactsBookUnitOfWork: IUnitOfWork
    {
        IContactsRepository ContactsRepository { get; }
    }
}
