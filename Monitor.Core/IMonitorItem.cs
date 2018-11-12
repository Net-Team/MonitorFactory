using System;

namespace Monitor.Core
{
    /// <summary>
    /// 定义监控项的接口
    /// </summary>
    public interface IMonitorItem : IDisposable
    {
        /// <summary>
        /// 获取监控项的别名
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        event Action<IMonitorItem, Exception> OnException;

        /// <summary>
        /// 启动
        /// </summary>
        void Start();
    }
}
