using System;
using Monitor.Core;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// Web监控选项
    /// </summary>
    public class WebSiteOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1d);

        /// <summary>
        /// 重试次数
        /// </summary>
        public int Retry { get; set; } = 3;

        /// <summary>
        /// 网站检测Uri
        /// </summary>
        public Uri Uri { get; set; }
    }
}
