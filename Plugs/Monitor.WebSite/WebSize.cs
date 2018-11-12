using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Monitor.WebSite
{
    /// <summary>
    /// 表示网站监控对象
    /// </summary>
    public class WebSize : IMonitor
    {
        /// <summary>
        /// 网站另名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 网站检测Url
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<IMonitor, Exception> OnException;
    }
}
