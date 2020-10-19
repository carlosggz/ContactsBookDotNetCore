using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF
{    
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ContactsBookContext _context;

        public EFUnitOfWork(ContactsBookContext context)
        {
            _context = context;
        }

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
