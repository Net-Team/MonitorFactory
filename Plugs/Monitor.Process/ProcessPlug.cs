using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Monitor.Process
{
    /// <summary>
    /// 程序监控插件
    /// </summary>
    public class ProcessPlug : MonitorPlug<ProcessInfo>
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task InvokeAsync(PlugContext context)
        {
            base.LogInfo($"{nameof(ProcessPlug)} 开始运行");
            var config = base.LoadJsonConfig<ProcessOption>();
            this.timer = new Timer((state) =>
               {
                   this.OnCheckMonitor(config.Monitors);
                   this.timer.Change(config.Interval * 1000, Timeout.Infinite);
               }, null, 0, Timeout.Infinite);
            await base.CompletedTask;
        }

        /// <summary>
        /// 检测监控队列
        /// </summary>
        /// <param name="processes">程序集合</param>
        /// <returns></returns>
        private void OnCheckMonitor(ProcessInfo[] processes)
        {
            foreach (var item in processes)
            {
                if (item.IsRunning() == false)
                {
                    item.Start();
                }
            }
        }
    }
}
