using Microsoft.Extensions.Logging;
using Monitor;
using System;
using Monitor.NotifyFactory;

namespace MonitorCenter
{
    class Program
    {
        static void Main(string[] args)
        {

            var consumer = new MonitorConsumer();
            var notifyFactory = new NotifyFactory();
            notifyFactory.AddEmailNotifyChannel((n) =>
            {
                n.Smtp = "mail.taichuan.com";
                n.SenderAccout = "iot@taichuan.com";
                n.SenderPassword = "tc123457";
                n.TargetEmails.Add("42309073@qq.com");
            });

            notifyFactory.AddHttpNotifyChannel((n) =>
            {
                n.Uri = new Uri("http://www.baidu.com");
            });

            var context = new PlugContext
            {
                LoggerFactory = new LoggerFactory().AddConsole().AddDebugger(),
                NotifyFactory = notifyFactory
            };
            consumer.RunAsync(context);
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
