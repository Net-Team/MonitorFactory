using Monitor.Core;
using System.Collections.Generic;

namespace Monitor.Plugs.Process
{
    /// <summary>
    /// 表示程序监控插件
    /// </summary>
    public class ProcessPlug : MonitorPlug<ProcessItem>
    {
        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<ProcessPlugConfig>();
            foreach (var item in config.Options)
            {
                yield return new ProcessItem(item);
            }
        }
    }
}
