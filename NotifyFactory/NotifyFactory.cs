using Monitor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.NotifyFactory
{

    /// <summary>
    /// 监控通知工厂
    /// </summary>
    public class NotifyFactory : INotifyFactory
    {

        /// <summary>
        /// 通知集合
        /// </summary>
        private List<INotifyChannel> notifyChannels = new List<INotifyChannel>();


        /// <summary>
        /// 添加通知通道
        /// </summary>
        /// <param name="channel"></param>
        public void AddChannel(INotifyChannel channel)
        {
            notifyChannels.Add(channel);
        }

        /// <summary>
        /// 执行通知
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task NotifyAsync(NotifyContent context)
        {
            foreach (var item in notifyChannels)
            {
                await item.NotifyAsync(context);
            }
        }
        //策略,重试,发送多次
    }
}
