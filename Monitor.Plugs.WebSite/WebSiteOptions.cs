using System;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// Web监控选项
    /// </summary>
    public class WebSiteOptions
    {
        /// <summary>
        /// 请求时间间隔
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// 请求超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 网站另名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 网站检测Uri
        /// </summary>
        public Uri Uri { get; set; }
    }
}
