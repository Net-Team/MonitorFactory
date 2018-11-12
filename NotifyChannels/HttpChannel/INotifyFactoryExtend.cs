using Monitor.HttpChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 表示 INotifyFactory 扩展
    /// </summary>
    public static class INotifyFactoryExtend
    {

        /// <summary>
        /// 添加Http通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        public static void AddHttpNotifyChannel(this INotifyFactory factory, Action<HttpChannelOptions> options)
        {
            var opt = new HttpChannelOptions();
            options?.Invoke(opt);

            var channel = new HttpChanneler(opt);
            factory.AddChannel(channel);
        }
    }
}
