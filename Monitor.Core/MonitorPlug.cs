using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Monitor.Core
{
    /// <summary>
    /// 表示监控插件基类
    /// </summary>
    public abstract class MonitorPlug<TMonitorItem> : IMonitorPlug where TMonitorItem : IMonitorItem
    {
        /// <summary>
        /// 监控项
        /// </summary>
        private IMonitorItem[] monitorItems;

        /// <summary>
        /// 延时timer
        /// </summary>
        private readonly Timer delayTimer;

        /// <summary>
        /// 文件监控
        /// </summary>
        private readonly FileSystemWatcher watcher;


        /// <summary>
        /// 获取插件上下文
        /// </summary>
        protected PlugContext Context { get; private set; }

        /// <summary>
        /// 配置文件变化后
        /// </summary>
        public event Action<IMonitorPlug> OnConfigChanged;

        /// <summary>
        /// 监控插件基类
        /// </summary>
        public MonitorPlug()
        {
            var dllFile = this.GetType().Assembly.Location;
            var jsonFile = Path.ChangeExtension(Path.GetFileName(dllFile), ".json");
            var path = Path.GetDirectoryName(dllFile);
            this.watcher = new FileSystemWatcher(path, jsonFile);
            this.watcher.NotifyFilter = NotifyFilters.LastWrite;
            this.watcher.EnableRaisingEvents = true;
            this.watcher.Changed += ConfigChanged;

            this.delayTimer = new Timer(this.OnTimerTick, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }


        /// <summary>
        /// 配置文件变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigChanged(object sender, FileSystemEventArgs e)
        {
            this.delayTimer.Change(TimeSpan.FromSeconds(1d), Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 配置文件变化时
        /// </summary>
        /// <param name="state"></param>
        private void OnTimerTick(object state)
        {
            var @event = this.OnConfigChanged;
            @event?.Invoke(this);
        }

        /// <summary>
        /// 加载json配置
        /// json文件为插件文件名.json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        protected TConfig LoadJsonConfig<TConfig>()
        {
            var file = Path.ChangeExtension(this.GetType().Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                throw new FileNotFoundException("找不到配置文件", file);
            }

            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<TConfig>(json);
        }

        /// <summary>
        /// 启动插件
        /// </summary>
        /// <param name="context">上下文</param>
        void IMonitorPlug.Start(PlugContext context)
        {
            var categoryName = this.GetType().Name;
            var logger = context.LoggerFactory.CreateLogger(categoryName);

            try
            {
                logger.LogInformation($"正在启动插件");
                this.Context = context;
                this.monitorItems = this.CreateMonitorItems().ToArray();

                foreach (var item in this.monitorItems)
                {
                    item.OnException += OnMonitorItemException;
                    item.Start();
                }
                logger.LogInformation($"插件启动成功");
            }
            catch (Exception ex)
            {
                logger.LogError(0, ex, "插件执行异常");
            }
        }


        /// <summary>
        /// 创建监控项的实例
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        protected abstract IEnumerable<IMonitorItem> CreateMonitorItems();


        /// <summary>
        /// 监控对象异常触发时
        /// </summary>
        /// <param name="item">监控对象</param>
        /// <param name="ex">异常消息</param>
        protected virtual async void OnMonitorItemException(IMonitorItem item, Exception ex)
        {
            this.Context
                 .LoggerFactory
                 .CreateLogger(item.Alias)
                 .LogError(0, ex, "监控对象遇到问题");

            var context = new NotifyContent
            {
                Title = $"[{item.Alias}] 监控提醒",
                Message = ex.ToString()
            };

            try
            {
                await this.Context.NotifyClientFactory.NotifyAsync(context);
            }
            catch (Exception exception)
            {
                this.Context
                    .LoggerFactory
                    .CreateLogger("LoggerFactory")
                    .LogError(0, exception, "通知工厂遇到问题");
            }
        }


        #region IDisposable
        /// <summary>
        /// 获取对象是否已释放
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 关闭和释放所有相关资源
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            this.IsDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MonitorPlug()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否也释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            this.watcher.EnableRaisingEvents = false;
            this.watcher.Dispose();

            if (this.monitorItems != null)
            {
                foreach (var item in this.monitorItems)
                {
                    item.Dispose();
                }
            }
        }
        #endregion
    }
}
