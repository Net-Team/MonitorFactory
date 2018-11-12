using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Monitor
{
    /// <summary>
    /// 表示监控对象基类
    /// </summary>
    public abstract class MonitorPlug<TMonitor> : IMonitorPlug where TMonitor : IMonitor
    {
#if NET451
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        protected readonly Task CompletedTask = Task.FromResult<object>(null);
#else
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        protected readonly Task CompletedTask = Task.CompletedTask;
#endif

        /// <summary>
        /// 监控对象日志
        /// </summary>
        private ILogger MonitorLogger;

        /// <summary>
        /// 插件上下文
        /// </summary>
        private PlugContext plugContext;

        /// <summary>
        /// 加载json配置
        /// json文件为插件文件名.json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        public TConfig LoadJsonConfig<TConfig>() where TConfig : IPlugOption<TMonitor>
        {
            var file = Path.ChangeExtension(this.GetType().Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                return default(TConfig);
            }

            var json = File.ReadAllText(file, Encoding.UTF8);
            var model = JsonConvert.DeserializeObject<TConfig>(json);
            foreach (var item in model.Monitors)
            {
                item.OnException += Monitor_OnException;
            }
            return model;

        }

        /// <summary>
        /// 输出异常日志
        /// </summary>
        /// <param name="message"></param>
        protected virtual void LogError(object message)
        {
            this.MonitorLogger.LogError(message?.ToString());
        }


        /// <summary>
        /// 输出异常日志
        /// </summary>
        /// <param name="message"></param>
        protected virtual void LogInfo(object message)
        {
            this.MonitorLogger.LogInformation(message?.ToString());
        }

        /// <summary>
        /// 异常触发
        /// </summary>
        /// <param name="monitor">监控对象</param>
        /// <param name="ex">异常消息</param>
        protected virtual void Monitor_OnException(IMonitor monitor, Exception ex)
        {
            this.LogError(ex.ToString());
            var context = new NotifyContent
            {
                Title = $"[{monitor.Alias}] 监控提醒",
                Message = ex.ToString()
            };
            this.plugContext.NotifyFactory.NotifyAsync(context).Wait();
        }

        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task IMonitorPlug.InvokeAsync(PlugContext context)
        {
            if (context != null)
            {
                this.plugContext = context;
                this.MonitorLogger = context.LoggerFactory.CreateLogger(this.GetType().Name);
                await this.InvokeAsync(context);
            }
        }

        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task InvokeAsync(PlugContext context);
    }
}
