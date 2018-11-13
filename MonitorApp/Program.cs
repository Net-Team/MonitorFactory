using Microsoft.Extensions.Logging;
using Monitor;
using Monitor.Factories;
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
            var context = new PlugContext
            {
                LoggerFactory = new LoggerFactory().AddConsole().AddDebugger(),
                NotifyClientFactory = new NotifyClientFactory().AddHttpClient(config.HttpOptions).AddMailClient(config.MailOptions)
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
