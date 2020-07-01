using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF
{    
    public class EFUnitOfWork : IContactsBookUnitOfWork
    {
        private readonly ContactsBookContext _context;
        private IContactsRepository _contactsRepository = null;

        public EFUnitOfWork(ContactsBookContext context)
        {
            _context = context;
        }

        public IContactsRepository ContactsRepository => _contactsRepository ?? (_contactsRepository = new EF.ContactsRepository(_context));
        public void CommitChanges()
        {
            _context.SaveChanges();
        }

        public void RollbackChanges()
        {
            //Nothing so far
        }

        public void StartChanges()
        {
            //Nothing so far
        } 
    }
}
