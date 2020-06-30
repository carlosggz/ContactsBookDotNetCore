using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public class SearchResults<T> where T: IDto
    {
        public int Total { get; private set; }
        public IEnumerable<T> Results { get; private set; }

        public SearchResults(int total, IEnumerable<T> results)
        {
            Total = total;
            Results = results;
        }
    }
}
