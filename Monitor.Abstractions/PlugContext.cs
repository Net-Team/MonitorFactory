using Microsoft.Extensions.Logging;

namespace Monitor
{
    /// <summary>
    /// 表示插件上下文
    /// </summary>
    public class PlugContext
    {
        /// <summary>
        /// 获取或设置日志工厂
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// 获取或通知工厂
        /// </summary>
        public INotifyClientFactory NotifyClientFactory { get; set; }
    }
}
