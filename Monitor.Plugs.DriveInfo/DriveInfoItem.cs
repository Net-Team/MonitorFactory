using Monitor.Core;
using System;
using System.Threading.Tasks;

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
        /// 磁盘监控对象
        /// </summary>
        /// <param name="options"></param>
        public DriveInfoItem(DriveInfoOptions options)
            : base(options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 检测磁盘
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            var drive = new System.IO.DriveInfo(this.options.DriveName);
            if (drive.IsReady == false)
            {
                Console.WriteLine($"磁盘 {this.options.DriveName} 未准备好");
            }
            else
            {
                //现剩余空间百分比
                var freeSpace = ((drive.TotalFreeSpace / (double)drive.TotalSize) * 100);

                //如果当前剩余比上一次小且 小于 限定值报警
                if (freeSpace < this.options.Residual)
                {
                    if (this.IsNotify == false)
                    {
                        this.IsNotify = true;
                        throw new Exception($"磁盘 {this.options.DriveName} 当前可用大小：{drive.TotalFreeSpace},可用空间已经不足{this.options.Residual}%");
                    }
                }
                else
                {
                    this.IsNotify = false;
                }
            }
#if NET45
            await Task.FromResult<object>(null);
#else
            await Task.CompletedTask;
#endif

        }
    }
}
