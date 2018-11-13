using System;
using System.Collections.Generic;
using Monitor.Core;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// Web监控选项
    /// </summary>
    public class WebSiteOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// 最大通知次数
        /// </summary>
        public int MaxNotify { get; set; } = 3;

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

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; } = "GET";

        /// <summary>
        /// 请求头
        /// </summary>
        public KeyValuePair<string, string>[] Header { get; set; } = new KeyValuePair<string, string>[0];

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 响应结果匹配
        /// </summary>
        public string ResultMatch { get; set; } = "*";
    }
}
