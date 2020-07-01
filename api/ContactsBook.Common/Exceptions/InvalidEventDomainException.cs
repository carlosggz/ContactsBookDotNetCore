using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Exceptions
{
    public class InvalidEventDomainException : DomainException
    {
        public InvalidEventDomainException(string message) 
            : base(message)
        {}
    }
}
