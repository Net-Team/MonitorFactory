using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 插件选项
    /// </summary>
    /// <typeparam name="TMonitor"></typeparam>
    public interface IPlugOption<TMonitor> where TMonitor : IMonitor
    {
        /// <summary>
        /// 获取或设置监控对象集合
        /// </summary>
        TMonitor[] Monitors { get; set; }
    }
}
