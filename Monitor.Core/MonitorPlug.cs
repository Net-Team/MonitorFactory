using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor.Core
{
    /// <summary>
    /// 监控插件抽象类
    /// </summary>
    public class MonitorPlug<TMonitor> : IMonitorPlug where TMonitor : IMonitor
    {

#if NET451
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        private static readonly Task CompletedTask = Task.FromResult<object>(null);
#else
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        private static readonly Task CompletedTask = Task.CompletedTask;
#endif


        /// <summary>
        /// 获取或设置插件上下文
        /// </summary>
        private PlugContext plugContext;

        /// <summary>
        /// 
        /// </summary>
        public MonitorPlugOptions<TMonitor> options;

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="context"></param>
        public void Start(PlugContext context)
        {
            this.plugContext = context;
            this.options = this.LoadJsonConfig<MonitorPlugOptions<TMonitor>>();
            foreach (var item in options.Monitors)
            {
                item.Logger = context.LoggerFactory.CreateLogger(typeof(TMonitor).Name);
                item.OnException += Item_OnException;
            }

            //this.LogInfo($"{this.GetType().Name} 开始监控");
            //this.timer = new Timer(async (state) =>
            //{
            //    await this.OnCheckMonitorAsync();
            //    this.timer.Change((Int64)this.Interval.TotalMilliseconds, Timeout.Infinite);
            //}, null, 0, Timeout.Infinite);
        }


        private void Item_OnException(IMonitor monitor, Exception ex)
        {
            var monitorException = new MonitorException(ex, monitor.Alias);
            this.plugContext.LoggerFactory.CreateLogger(typeof(TMonitor).Name).LogError(ex.ToString());
            NotifyAsync(monitorException).Wait();
        }


        /// <summary>
        /// 停止监控
        /// </summary>
        public void Stop()
        {
        }


        /// <summary>
        /// 监控通知异常
        /// </summary>
        /// <param name="exception">异常消息</param>
        /// <returns></returns>
        protected virtual async Task NotifyAsync(MonitorException exception)
        {
            var content = new NotifyContent
            {
                Title = $"[{exception.Alias}] 监控提醒",
                Message = exception.ToString()
            };
            await this.plugContext.NotifyFactory.NotifyAsync(content);
        }


        /// <summary>
        /// 加载json配置
        /// json文件为插件文件名.json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        public TConfig LoadJsonConfig<TConfig>()
        {
            var file = Path.ChangeExtension(this.GetType().Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                return default(TConfig);
            }

            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<TConfig>(json);
        }
    }
}
