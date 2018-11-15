using System;

namespace Monitor
{
    /// <summary>
    /// 定义监控插件的接口
    /// </summary>
    public interface IMonitorPlug : IDisposable
    {
        /// <summary>
        /// 配置文件变化后
        /// </summary>
        event Action<IMonitorPlug> OnConfigChanged;

        /// <summary>
        /// 启动插件
        /// </summary>
        /// <param name="context">上下文</param>
        void Start(PlugContext context);
    }
}
