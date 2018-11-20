using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.Processor
{
    /// <summary>
    /// 表示处理器监控插件
    /// </summary>
    public class ProcessorPlug : MonitorPlug<ProcessorItem>
    {

        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<ProcessorConfig>();
            foreach (var item in config.Options)
            {
                yield return new ProcessorItem(item);
            }
        }
    }
}
