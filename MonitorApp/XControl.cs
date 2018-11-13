using Microsoft.Extensions.Logging;
using Monitor;
using Monitor.Core;
using System.Linq;
using Topshelf;

namespace MonitorApp
{
    class XControl : ServiceControl
    {
        /// <summary>
        /// 所有插件
        /// </summary>
        private IMonitorPlug[] plugs;

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

            var context = new PlugContext
            {
                NotifyClientFactory = notifyClientFactory,
                LoggerFactory = new LoggerFactory().AddConsole().AddDebugger()
            };

            this.plugs = Plugs.FindMonitorPlugs().ToArray();
            foreach (var item in plugs)
            {
                item.Start(context);
            }

            return true;
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
                item.Dispose();
            }
            return true;
        }
    }
}
