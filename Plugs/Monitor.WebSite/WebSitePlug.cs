using System;
using System.Threading.Tasks;

namespace Monitor.WebSite
{
    /// <summary>
    /// 表示 Web站点监控插件
    /// </summary>
    public class WebSitePlug : MonitorPlug<WebSize>
    {
        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task InvokeAsync(PlugContext context)
        {
            base.LogInfo($"{nameof(WebSitePlug)} 开始运行");
            await base.CompletedTask;
        }
    }
}
