using Monitor.Core;
using System.Collections.Generic;

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
