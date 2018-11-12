using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monitor.Core
{
    /// <summary>
    /// 表示监控对象基类
    /// </summary>
    public abstract class MonitorPlug<TMonitorItem> : IMonitorPlug where TMonitorItem : IMonitorItem
    {
        /// <summary>
        /// 监控项
        /// </summary>
        private IMonitorItem[] monitorItems;

        /// <summary>
        /// 获取插件上下文
        /// </summary>
        protected PlugContext Context { get; private set; }

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
            try
            {
                this.Context = context;
                this.monitorItems = this.CreateMonitorItems().ToArray();

                foreach (var item in this.monitorItems)
                {
                    item.OnException += OnMonitorItemException;
                    item.Start();
                }
            }
            catch (Exception ex)
            {
                var categoryName = this.GetType().Name;
                var logger = context.LoggerFactory.CreateLogger(categoryName);
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
