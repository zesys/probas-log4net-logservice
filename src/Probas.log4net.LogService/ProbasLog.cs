using System.Text.Json.Serialization;

namespace Probas.log4net.LogService
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class ProbasLog
    {
        /// <summary>
        /// 记录日志时的时间
        /// </summary>
        [JsonPropertyName("log_timestamp")]
        public long LogTimestamp { get; set; }

        /// <summary>
        /// 应用Key
        /// </summary>
        [JsonPropertyName("app_key")]
        public string AppKey { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        [JsonPropertyName("host_name")]
        public string HostName { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        [JsonPropertyName("level")]
        public string Level { get; set; }

        /// <summary>
        /// 日志名
        /// </summary>
        [JsonPropertyName("logger_name")]
        public string LoggerName { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [JsonPropertyName("exception")]
        public ProbasLogException Exception { get; set; }
    }
}
