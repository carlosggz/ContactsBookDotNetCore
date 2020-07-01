using ContactsBook.Common.Logger;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Infrastructure.Logger
{
    public class NLogLogger : IAppLogger
    {
        private readonly ILogger _logger = null;

        public NLogLogger(string loggerName)
            => _logger = LogManager.GetLogger(loggerName);

        public void Debug(string message) => _logger.Debug(message);
        public void Error(string message) => _logger.Error(message);
        public void Info(string message) => _logger.Info(message);
        public void Error(Exception exception, string message = null)
        {
            var msg = new StringBuilder();
            msg.AppendLine((message ?? "Message") + ": " + exception.Message);

            var inner = exception.InnerException;

            while (inner != null)
            {
                msg.AppendLine("Inner exception: " + inner.Message);
                inner = inner.InnerException;
            }

            msg.AppendLine("Stack trace: " + exception.StackTrace);

            _logger.Error(msg.ToString());
        }
    }
}
