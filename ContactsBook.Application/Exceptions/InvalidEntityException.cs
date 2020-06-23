using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Application.Exceptions
{
    public class InvalidEntityException: Exception
    {
        public InvalidEntityException(string errorMessage)
            :base(errorMessage)
        {}
    }
}
