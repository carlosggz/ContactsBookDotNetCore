using ContactsBook.Application.Dtos;
using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services
{
    public class ContactsService: BaseService
    {
        public ContactsService(IUnitOfWork unitOfWork, IEventBus domainBus) 
            : base(unitOfWork, domainBus)
        {}

        public void AddContact(ContactsModel model)
        { }

        public void UpdateContact(string id, ContactsModel model)
        { }

        public void DeleteContact(string id)
        { }
    }
}
