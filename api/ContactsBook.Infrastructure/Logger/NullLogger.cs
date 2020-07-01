using ContactsBook.Common.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Logger
{
    public class NullLogger : IAppLogger
    {
        public void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

        public void Error(string message) => System.Diagnostics.Debug.WriteLine(message);

        public void Error(Exception exception, string message = null) 
            => System.Diagnostics.Debug.WriteLine((message == null ? string.Empty : message) + ": " + exception?.Message);

        public void Info(string message) => System.Diagnostics.Debug.WriteLine(message);
    }
}
