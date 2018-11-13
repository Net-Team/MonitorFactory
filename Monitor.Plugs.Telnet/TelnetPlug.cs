using Monitor.Core;
using System;
using System.Collections.Generic;

namespace Monitor.Plugs.Telnet
{
    /// <summary>
    /// Telnet 端口监控插件
    /// </summary>
    public class TelnetPlug : MonitorPlug<TelnetItem>
    {

        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<TelnetPlugConfig>();
            foreach (var item in config.Options)
            {
                yield return new TelnetItem(item);
            }
        }
    }
}
