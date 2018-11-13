using Monitor.Core;
using System.Collections.Generic;

namespace Monitor.Plugs.Service
{
    public class ServicePlug : MonitorPlug<ServiceItem>
    {
        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<ServicePlugConfig>();
            foreach (var item in config.Options)
            {
                yield return new ServiceItem(item);
            }
        }
    }
}
