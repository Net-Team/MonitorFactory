using Monitor.Core;
using System.Collections.Generic;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// 表示 Web站点监控插件
    /// </summary>
    public class WebSitePlug : MonitorPlug<WebSiteItem>
    {
        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<WebSitePlugConfig>();
            foreach (var item in config.Options)
            {
                yield return new WebSiteItem(item);
            }
        }
    }
}
