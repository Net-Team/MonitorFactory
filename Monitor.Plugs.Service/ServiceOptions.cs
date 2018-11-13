using Monitor.Core;

namespace Monitor.Plugs.Service
{
    /// <summary>
    /// 服务选项
    /// </summary>
    public class ServiceOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// 获取或设置服务名称
        /// </summary>
        public string Name { get; set; }
    }
}
