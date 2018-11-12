using Monitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MonitorCenter
{
    /// <summary>
    /// 提供插件查找
    /// </summary>
    public static class Plugs
    {
        /// <summary>
        /// 查找所有插件
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IMonitorPlug> FindMonitorPlugs()
        {
            var baseType = typeof(IMonitorPlug);
            var outterPlugs = Directory
                 .GetDirectories("Plugs")
                 .Select(dir => $"{dir}\\{dir.Split('\\').Last()}.dll")
                 .Select(file => Assembly.LoadFrom(file))
                 .SelectMany(item => item.GetTypes())
                 .Where(item => item.IsPublic && baseType.IsAssignableFrom(item))
                 .ToArray();

            foreach (var item in outterPlugs)
            {
                yield return Activator.CreateInstance(item) as IMonitorPlug;
            }
        }
    }
}
