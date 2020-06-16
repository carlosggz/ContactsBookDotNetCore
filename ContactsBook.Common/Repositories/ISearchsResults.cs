using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public interface ISearchsResults<T> where T: IDto
    {
        int Total { get; }
        IEnumerable<T> Results { get; }
    }
}
