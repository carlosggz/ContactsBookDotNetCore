using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Services
{
    public class BaseService
    {
        public BaseService(IUnitOfWork unitOfWork, IEventBus domainBus)
        {
            UnitOfWork = unitOfWork;
            DomainBus = domainBus;
        }

        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IEventBus DomainBus { get; private set; }
    }
}
