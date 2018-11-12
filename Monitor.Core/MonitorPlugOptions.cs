using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core
{
    /// <summary>
    /// 定义chajian接口
    /// </summary>
    public class MonitorPlugOptions<TMonitor> where TMonitor : IMonitor
    {
        /// <summary>
        /// 获取或设置监控对象集合
        /// </summary>
        public TMonitor[] Monitors { get; set; }
    }
}
