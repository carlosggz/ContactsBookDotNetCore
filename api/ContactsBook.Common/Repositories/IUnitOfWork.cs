using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public interface IUnitOfWork
    {
        void StartChanges();
        void CommitChanges();
        void RollbackChanges();
    }
}
