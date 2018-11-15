using Microsoft.Extensions.Logging;
using Monitor;
using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Topshelf;

namespace MonitorApp
{
    class XControl : ServiceControl
    {
        /// <summary>
        /// 上下文
        /// </summary>
        private PlugContext context;

        /// <summary>
        /// 所有插件
        /// </summary>
        private Dictionary<Type, IMonitorPlug> plugs;

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            var config = Config.Load();
            var notifyClientFactory = new NotifyClientFactory();
            if (config.MailOptions != null)
            {
                notifyClientFactory.AddMailClient(config.MailOptions);
            }
            if (config.HttpOptions != null)
            {
                notifyClientFactory.AddHttpClient(config.HttpOptions);
            }

            this.context = new PlugContext
            {
                NotifyClientFactory = notifyClientFactory,
                LoggerFactory = new LoggerFactory().AddConsole().AddDebugger()
            };

            this.plugs = Plugs
                .FindMonitorPlugs()
                .ToDictionary(item => item.GetType(), item => item);

            foreach (var item in plugs)
            {
                item.Value.OnConfigChanged += OnConfigChanged;
                item.Value.Start(this.context);
            }

            return true;
        }

        /// <summary>
        /// 插件配置文件变化
        /// </summary>
        /// <param name="item"></param>
        private void OnConfigChanged(IMonitorPlug item)
        {
            var type = item.GetType();
            var plug = Activator.CreateInstance(type) as IMonitorPlug;

            plug.OnConfigChanged += OnConfigChanged;
            plug.Start(this.context);

            this.plugs[type] = plug;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            foreach (var item in this.plugs)
            {
                item.Value.Dispose();
            }
            return true;
        }
    }
}
