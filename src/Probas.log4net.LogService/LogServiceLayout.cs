using log4net.Core;
using log4net.Layout;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Probas.log4net.LogService
{
    public class LogServiceLayout : LayoutSkeleton
    {
        /// <summary>
        /// 应用id（用于日志标识）
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 默认 IgnoresException 为 true，这里设置为 false
        /// </summary>
        public override void ActivateOptions()
        {
            IgnoresException = false;
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var log = GetProbasLog(loggingEvent);
            var message = JsonSerializer.Serialize(log, ProbasContent.JsonSerializerOptions);
            writer.Write(message);
        }

        private ProbasLog GetProbasLog(LoggingEvent loggingEvent)
        {
            var log = new ProbasLog
            {
                LogTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                AppKey = AppKey,
                HostName = Dns.GetHostName(),
                Level = loggingEvent.Level.ToString(),
                LoggerName = loggingEvent.LoggerName,
                Message = loggingEvent.RenderedMessage
            };

            if (loggingEvent.ExceptionObject != null)
            {
                var exception = new ProbasLogException
                {
                    Name = loggingEvent.ExceptionObject.GetType().ToString(),
                    Message = loggingEvent.ExceptionObject.Message,
                    StackTrace = loggingEvent.ExceptionObject.StackTrace
                };
                var logException = exception;
                var innerException = loggingEvent.ExceptionObject.InnerException;
                while (innerException != null)
                {
                    logException.InnerException = new ProbasLogException
                    {
                        Name = innerException.GetType().ToString(),
                        Message = innerException.Message,
                        StackTrace = innerException.StackTrace
                    };
                    innerException = innerException.InnerException;
                    logException = logException.InnerException;
                }
                log.Exception = exception;
            }
            return log;
        }
    }
}
