using ContactsBook.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public abstract class BaseSearchCriteria : ISearchCriteria
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public BaseSearchCriteria(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new DomainException("Invalid search parameters");

            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
