using System.Text.Json.Serialization;

namespace Probas.log4net.LogService
{
    /// <summary>
    /// 异常信息实体
    /// </summary>
    public class ProbasLogException
    {
        /// <summary>
        /// 类名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        [JsonPropertyName("stack_trace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// 内部错误
        /// </summary>
        [JsonPropertyName("inner_exception")]
        public ProbasLogException InnerException { get; set; }
    }
}
