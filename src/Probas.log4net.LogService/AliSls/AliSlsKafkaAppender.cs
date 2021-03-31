using Confluent.Kafka;
using log4net.Appender;
using log4net.Core;
using System;

namespace Probas.log4net.LogService
{
    public class AliSlsKafkaAppender : AppenderSkeleton
    {

        private IProducer<Null, string> _producer;

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

                //Initial
                if (_producer == null)
                {
                    var config = new ProducerConfig
                    {
                        BootstrapServers = Settings.Endpoint,
                        SaslMechanism = SaslMechanism.Plain,
                        SecurityProtocol = SecurityProtocol.SaslSsl,
                        SaslUsername = Settings.Project,
                        SaslPassword = $"{Settings.AccessKey}#{Settings.SecretKey}"
                    };

                    _producer = new ProducerBuilder<Null, string>(config).Build();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("could not init producer", ex);
            }
        }


        protected override void OnClose()
        {
            base.OnClose();
            try
            {
                _producer?.Dispose();
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("could not dispose producer", ex);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var probasContent = new ProbasContent(loggingEvent, Layout);
            try
            {
                _producer.Produce(Settings.Logstore, new Message<Null, string> { Value = probasContent.Content }, delivery =>
                {
                    if (delivery.Error.IsError)
                    {
                        var error = new Exception(delivery.Error.Reason);
                        var msg = $"producer delivery error {delivery.Topic}";
                        ErrorHandler.Error(msg, error);
                        Console.WriteLine(msg, error);
                    }
                    else
                    {
                        Console.WriteLine($"producer delivered message to {delivery.TopicPartitionOffset}");
                    }
                });
            }
            catch (Exception ex)
            {
                var msg = $"producer delivery error {Settings.Logstore}";
                ErrorHandler.Error(msg, ex);
                Console.WriteLine(msg, ex);
            }
        }
    }
}
