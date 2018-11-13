using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.Telnet
{
    /// <summary>
    /// Telnet 监控选项
    /// </summary>
    public class TelnetOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// IP 地址/域名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 80;

        /// <summary>
        /// 异常通知间隔
        /// 不设置只通知一次
        /// </summary>
        public TimeSpan[] NotifyTimeSpan { get; set; } = new TimeSpan[0];
    }
}
