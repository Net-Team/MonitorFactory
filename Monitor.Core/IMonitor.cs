using System;

namespace Monitor.Core
{
    /// <summary>
    /// 定义监控的对象
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// 获取别名
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// 日志
        /// </summary>
        Microsoft.Extensions.Logging.ILogger Logger { set; }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        event Action<IMonitor, Exception> OnException;
    }
}