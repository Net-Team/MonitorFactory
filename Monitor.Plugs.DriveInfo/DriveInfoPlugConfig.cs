using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.DriveInfo
{
    /// <summary>
    /// 表示磁盘监控插件配置
    /// </summary>
    class DriveInfoPlugConfig
    {
        /// <summary>
        /// 监控磁盘集合
        /// </summary>
        public DriveInfoOptions[] Options { get; set; }
    }
}
