using System;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 表示监控插件
    /// </summary>
    public interface IMonitorPlug
    {
        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task InvokeAsync(PlugContext context);
    }
}
