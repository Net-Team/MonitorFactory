using Microsoft.Extensions.Logging;

namespace Monitor
{
    /// <summary>
    /// 插件上下文
    /// </summary>
    public class PlugContext
    {
        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// 通知工厂
        /// </summary>
        public INotifyFactory NotifyFactory { get; set; }
    }
}
