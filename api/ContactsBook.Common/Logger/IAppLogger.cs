using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.Logger
{
    public interface IAppLogger
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception exception, string message = null);
    }
}
