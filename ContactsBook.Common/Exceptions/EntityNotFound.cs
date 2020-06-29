using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Exceptions
{
    public class EntityNotFound : DomainException
    {
        public EntityNotFound(string message)
            : base(message)
        { }
    }
}
