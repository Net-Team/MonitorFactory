using Topshelf;

namespace MonitorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<XControl>();
                x.RunAsLocalSystem();
                x.SetServiceName(nameof(MonitorApp));
                x.SetDisplayName(nameof(MonitorApp));
                x.SetDescription(nameof(MonitorApp));
            });
        }
    }
}
