using System;
using System.Collections.Generic;

namespace Monitor.NotifyClients.Http
{
    /// <summary>
    /// Http 消息通知选项
    /// </summary>
    public class NotifyClientOptions
    {
        /// <summary>
        /// 或取或设置接收者接口地址
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 获取请求头集合
        /// </summary>
        public List<KeyValuePair<string, string>> Header { get; } = new List<KeyValuePair<string, string>>();
    }
}
