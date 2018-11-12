using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 
    /// </summary>
    public class PlugOption<TMonitor> : IPlugOption<TMonitor> where TMonitor : IMonitor
    {
        /// <summary>
        /// 监控插件选项
        /// </summary>
        public TMonitor[] Monitors { get; set; }
    }
}
