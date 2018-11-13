using Monitor.Core;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Monitor.Plugs.DriveInfo
{
    /// <summary>
    /// 表示磁盘监控对象
    /// </summary>
    public class DriveInfoItem : TimerMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly DriveInfoOptions options;

        /// <summary>
        /// 是否通知过一次
        /// </summary>
        private bool IsNotify = false;

        /// <summary>
        /// 磁盘信息
        /// </summary>
        private System.IO.DriveInfo driveInfo;

        /// <summary>
        /// 上一次空闲比例
        /// </summary>
        private double lastFreeSpace = 100d;


        /// <summary>
        /// 磁盘监控对象
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public DriveInfoItem(DriveInfoOptions options)
            : base(options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            this.driveInfo = new System.IO.DriveInfo(this.options.DriveName);
            if (driveInfo.IsReady == false)
            {
                throw new ArgumentException($"磁盘 {this.options.DriveName} 未准备好");
            }

        }

        /// <summary>
        /// 检测磁盘
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            //现剩余空间百分比
            var freeSpace = ((driveInfo.TotalFreeSpace / (double)driveInfo.TotalSize) * 100);


            foreach (var residual in this.options.Residuals.OrderBy(item => item))
            {
                if (lastFreeSpace > residual && residual > freeSpace)
                {
                    lastFreeSpace = freeSpace;
                    throw new Exception($"磁盘 {this.options.DriveName} 当前可用大小：{driveInfo.TotalFreeSpace / 1024 / 1024}M,可用空间已经不足 {residual}%.");
                }
            }
            lastFreeSpace = freeSpace;
#if NET45
            await Task.FromResult<object>(null);
#else
            await Task.CompletedTask;
#endif

        }
    }
}
