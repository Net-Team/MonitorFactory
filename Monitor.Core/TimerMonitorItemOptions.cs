using System;

namespace Monitor.Core
{
    /// <summary>
    /// 表示基于定时器的监控对象选项
    /// </summary>
    public class TimerMonitorItemOptions
    {
        /// <summary>
        /// 请求时间间隔
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30d);

        /// <summary>
        /// 网站另名
        /// </summary>
        public string Alias { get; set; }
    }
}
