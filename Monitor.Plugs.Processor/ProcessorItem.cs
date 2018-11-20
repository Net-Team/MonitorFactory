using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Plugs.Processor
{
    /// <summary>
    /// 表示处理器监控对象
    /// </summary>
    public class ProcessorItem : TimerMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly ProcessorOptions options;

        /// <summary>
        /// 每次检测间隔时间,默认1秒
        /// </summary>
        private readonly TimeSpan dueTime = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 持续多少次触发报警才报警
        /// </summary>
        private readonly int DurationTimes;

        /// <summary>
        /// 已经触发次数
        /// </summary>
        private int TriggerCounter = 0;

        /// <summary>
        /// 第一次通知时间
        /// </summary>
        private DateTime firstNotifyTime = DateTime.MinValue;

        /// <summary>
        /// 上一次通知时间戳
        /// </summary>
        private TimeSpan lastTimeSpan = TimeSpan.Zero;

        /// <summary>
        /// 构造处理器监控对象
        /// </summary>
        /// <param name="options"></param>
        public ProcessorItem(ProcessorOptions options)
            : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.NotifyTimeSpan.Length == 0)
            {
                options.NotifyTimeSpan = new TimeSpan[] { TimeSpan.FromSeconds(30) };
            }

            options.Interval = dueTime;
            //计算持续多少次进行判断
            this.DurationTimes = (int)(options.Duration.TotalMilliseconds / this.dueTime.TotalMilliseconds);
            this.options = options;
        }

        /// <summary>
        /// 检测Cpu 使用情况
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            var cuUsage = ProcessorInfo.CpuUsage;
            if (cuUsage > this.options.MaxUsage)
            {
                this.TriggerCounter += 1;
            }
            else
            {
                this.TriggerCounter = 0;
                firstNotifyTime = DateTime.MinValue;
                lastTimeSpan = TimeSpan.Zero;
            }

            if (this.TriggerCounter >= this.DurationTimes)
            {

                if (this.firstNotifyTime == DateTime.MinValue)
                {
                    this.firstNotifyTime = DateTime.Now;
                    throw new Exception($"{DateTime.Now} 当前CPU 使用率持续 {this.options.Duration.TotalSeconds} 秒使用大于 {this.options.MaxUsage}%,当前实时值：{(int)cuUsage} %");
                }

                var curTimeSpan = DateTime.Now.Subtract(firstNotifyTime);

                //当配置1个或多个间隔时间的时候
                foreach (var target in this.options.NotifyTimeSpan.OrderBy(item => item))
                {
                    if (curTimeSpan > target && target > lastTimeSpan)
                    {
                        lastTimeSpan = curTimeSpan;
                        throw new Exception($"{DateTime.Now} 当前CPU 使用率持续 {this.options.Duration.TotalSeconds} 秒使用大于 {this.options.MaxUsage}%,当前实时值：{(int)cuUsage} %");
                    }
                }
            }
            await Task.FromResult<object>(null);
        }
    }
}
