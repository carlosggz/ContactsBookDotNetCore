using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common
{
    public interface IUnitOfWork
    {
        void CommitChanges();
        void RollbackChanges();
    }
}
