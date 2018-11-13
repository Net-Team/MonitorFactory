using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.DriveInfo
{
    /// <summary>
    /// 表示磁盘监控选项
    /// </summary>
    public class DriveInfoOptions : TimerMonitorItemOptions
    {
        /// <summary>
        /// 获取磁盘名称
        /// A到Z的字母
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// 剩余百分比
        /// </summary>
        public int[] Residuals { get; set; }
    }
}
