using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.DriveInfo
{
    /// <summary>
    /// 表示磁盘监控插件
    /// </summary>
    public class DriveInfoPlug : MonitorPlug<DriveInfoItem>
    {
        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IMonitorItem> CreateMonitorItems()
        {
            var config = this.LoadJsonConfig<DriveInfoPlugConfig>();
            foreach (var item in config.Options)
            {
                yield return new DriveInfoItem(item);
            }
        }
    }
}
