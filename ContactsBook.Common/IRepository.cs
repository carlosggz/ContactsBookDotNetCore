using ContactsBook.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common
{
    public interface IRepository<TEntity> where TEntity: IEntity
    {
    }
}
