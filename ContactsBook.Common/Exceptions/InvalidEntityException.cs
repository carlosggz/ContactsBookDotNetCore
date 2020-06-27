using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Exceptions
{
    public class InvalidEntityException: DomainException
    {
        public InvalidEntityException(string message)
            :base(message)
        {}
    }
}
