using Monitor.Core;
using System;

namespace Monitor.Plugs.Processor
{
    /// <summary>
    /// 表示处理器监控选项
    /// </summary>
    public class ProcessorOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// 最大使用率
        /// </summary>
        public int MaxUsage { get; set; } = 95;

        /// <summary>
        /// 持续时长,默认30秒
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(30d);

        /// <summary>
        /// 异常通知间隔
        /// 不设置只通知一次
        /// </summary>
        public TimeSpan[] NotifyTimeSpan { get; set; } = new TimeSpan[0];
    }
}
