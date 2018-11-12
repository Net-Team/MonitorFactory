using Microsoft.Extensions.Logging;
using Monitor;
using Monitor.Factories;
using System;
using System.Linq;

namespace MonitorCenter
{
    class Program
    {
        static IMonitorPlug[] plugs;

        static void Main(string[] args)
        {
            var notifyClientFactory = new NotifyClientFactory().AddClient(http =>
            {
                http.Uri = new Uri("http://www.baidu.com");
            })
            .AddClient(mail =>
            {
                mail.Smtp = "mail.taichuan.com";
                mail.SenderAccout = "iot@taichuan.com";
                mail.SenderPassword = "tc123457";
                mail.TargetEmails.Add("42309073@qq.com");
                mail.TargetEmails.Add("366193849@qq.com");
            });

            var context = new PlugContext
            {
                LoggerFactory = new LoggerFactory().AddConsole().AddDebugger(),
                NotifyClientFactory = notifyClientFactory
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
