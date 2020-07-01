using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Repositories.EF
{
    public abstract class BaseEFRepository
    {
        protected ContactsBookContext Context { get; private set; }

        public BaseEFRepository(ContactsBookContext context)
            => Context = context;
    }
}
