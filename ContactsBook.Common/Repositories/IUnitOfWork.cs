using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public interface IUnitOfWork
    {
        void CommitChanges();
        void RollbackChanges();
    }
}
