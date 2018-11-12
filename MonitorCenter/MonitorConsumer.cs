using Microsoft.Extensions.Logging;
using Monitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonitorCenter
{
    public class MonitorConsumer
    {
        /// <summary>
        /// 插件列表
        /// </summary>
        private readonly List<IMonitorPlug> plugList = new List<IMonitorPlug>();

        public MonitorConsumer()
        {
            var baseType = typeof(IMonitorPlug); //.MakeGenericType(typeof(IMonitorPlug));
            var outterPlugs = Directory
                 .GetDirectories("Plugs")
                 .Select(dir => $"{dir}\\{dir.Split('\\').Last()}.dll")
                 .Select(file => Assembly.LoadFrom(file))
                 .SelectMany(item => item.GetTypes())
                 .Where(item => item.IsPublic && baseType.IsAssignableFrom(item))
                 .ToArray();

            foreach (var item in outterPlugs)
            {
                var plug = Activator.CreateInstance(item) as IMonitorPlug;
                this.AddPlug(plug);
            }
        }

        ///// <summary>
        ///// 自动搜索和添加插件
        ///// </summary>
        ///// <returns></returns>
        //public MonitorConsumer AutoAddPlugs()
        //{
        //    var baseType = typeof(IMonitorPlug); //.MakeGenericType(typeof(IMonitorPlug));
        //    var outterPlugs = Directory
        //         .GetDirectories("Plugs")
        //         .Select(dir => $"{dir}\\{dir.Split('\\').Last()}.dll")
        //         .Select(file => Assembly.LoadFrom(file))
        //         .SelectMany(item => item.GetTypes())
        //         .Where(item => item.IsPublic && baseType.IsAssignableFrom(item))
        //         .ToArray();

        //    foreach (var item in outterPlugs)
        //    {
        //        var plug = Activator.CreateInstance(item) as IMonitorPlug;
        //        this.AddPlug(plug);
        //    }
        //    return this;
        //}

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="plug">插件</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddPlug(IMonitorPlug plug)
        {
            if (plug == null)
            {
                throw new ArgumentNullException(nameof(plug));
            }
            this.plugList.Add(plug);
        }

        /// <summary>
        /// 运行插件
        /// </summary>
        /// <param name="context"></param>
        public async void RunAsync(PlugContext context)
        {
            var tasks = plugList.Select(plug => plug.InvokeAsync(context));
            await Task.WhenAll(tasks);
        }
    }
}
