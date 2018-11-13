using Microsoft.Extensions.Logging;
using Monitor;
using Monitor.Core;
using System;
using System.Linq;

namespace MonitorApp
{
    class Program
    {
        static IMonitorPlug[] plugs;

        static void Main(string[] args)
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

            plugs = Plugs.FindMonitorPlugs().ToArray();
            foreach (var item in plugs)
            {
                item.Start(context);
            }

            Console.WriteLine("启动完成!");
            Console.ReadKey();
        }
    }
}
