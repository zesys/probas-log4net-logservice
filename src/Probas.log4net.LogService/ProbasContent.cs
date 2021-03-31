using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Probas.log4net.LogService
{
    public class ProbasContent
    {
        private const string detailKey = "__detail_message__";
        private const string detailValue = "{message}";
        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly Dictionary<string, string> _contents;
        private readonly string _message;

        public ProbasContent(LoggingEvent loggingEvent, ILayout layout)
        {
            using var sr = new StringWriter();
            layout.Format(sr, loggingEvent);
            _message = sr.ToString();

            _contents = new Dictionary<string, string>
            {
                { "__domain__", loggingEvent.Domain },
                { "__fix__", loggingEvent.Fix.ToString() },
                { "__identity__", loggingEvent.Identity },
                { "__level__", loggingEvent.Level.ToString() },
                { "__logger_name__", loggingEvent.LoggerName },
                { "__user_name__", loggingEvent.UserName },
                { "__time_stamp__", loggingEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF") },
                { "__message__", loggingEvent.RenderedMessage },
                { detailKey, detailValue },
            };
            if (layout.IgnoresException && loggingEvent.ExceptionObject != null)
                _contents.Add("__exception_string__", loggingEvent.GetExceptionString());

            Console.WriteLine(Content);
        }

        public Dictionary<string, string> Contents => new(_contents)
        {
            [detailKey] = _message
        };

        public string Content => JsonSerializer.Serialize(_contents, JsonSerializerOptions).Replace($"\"{detailValue}\"", _message);
    }
}
