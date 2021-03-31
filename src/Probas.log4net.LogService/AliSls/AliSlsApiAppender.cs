using Aliyun.Api.LogService.Domain.Log;
using log4net.Appender;
using log4net.Core;
using Probas.Aliyun.LogService;
using System;
using System.Collections.Generic;

namespace Probas.log4net.LogService
{
    public class AliSlsApiAppender : AppenderSkeleton
    {
        private ILogServiceClient _client;

        /// <summary>
        /// 连接地址和主题配置
        /// </summary>
        public LogSettings Settings { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            try
            {
                if (Settings == null)
                    throw new LogException("Settings is missing");

                if (string.IsNullOrEmpty(Settings.Endpoint))
                    throw new Exception("Endpoint is missing");

                if (string.IsNullOrEmpty(Settings.Project))
                    throw new Exception("Project is missing");

                if (string.IsNullOrEmpty(Settings.Logstore))
                    throw new Exception("Logstore is missing");

                if (string.IsNullOrEmpty(Settings.AccessKey))
                    throw new Exception("AccessKey is missing");

                if (string.IsNullOrEmpty(Settings.SecretKey))
                    throw new Exception("SecretKey is missing");

                if (string.IsNullOrEmpty(Settings.Topic))
                    throw new Exception("Topic is missing");

                //Initial
                if (_client == null)
                {
                    _client = LogServiceClientBuilders.HttpBuilder.Endpoint(Settings.Endpoint, Settings.Project).Credential(Settings.AccessKey, Settings.SecretKey).Build();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("could not init producer", ex);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var probasContent = new ProbasContent(loggingEvent, Layout);
            var logs = new List<LogInfo>
            {
                new LogInfo
                {
                    Time = DateTimeOffset.Now,
                    Contents = probasContent.Contents
                }
            };
            var tags = new Dictionary<string, string>();
            var source = "netapi";
            var logGroupInfo = new LogGroupInfo { Logs = logs, LogTags = tags, Topic = Settings.Topic, Source = source };
            var request = new PostLogsRequest(Settings.Logstore, logGroupInfo);
            var result = _client.PostLogStoreLogsAsync(request).Result;
            if (!result.IsSuccess)
            {
                var error = new Exception(result.Error.ToString());
                var msg = $"post log store logs failed {result.RequestId}";
                ErrorHandler.Error(msg, error);
                Console.WriteLine(msg, error);
            }
            else
            {
                Console.WriteLine($"post log store logs request id {result.RequestId}");
            }
        }
    }
}
