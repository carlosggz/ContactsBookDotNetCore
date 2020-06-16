using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public interface ISearchCriteria
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}
