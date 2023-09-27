using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class LoggersManager : ILoggersManager
    {
       private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.Error(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }
    }
}
