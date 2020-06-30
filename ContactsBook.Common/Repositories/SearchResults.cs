using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Repositories
{
    public class SearchResults<T> where T: IDto
    {
        public readonly int Total;
        public readonly IEnumerable<T> Results;

        public SearchResults(int total, IEnumerable<T> results)
        {
            Total = total;
            Results = results;
        }
    }
}
