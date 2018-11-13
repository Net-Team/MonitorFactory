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
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 网站检测Uri
        /// </summary>
        public Uri Uri { get; set; }
    }
}
