using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Factories
{
    /// <summary>
    /// 表示监控通知工厂
    /// </summary>
    public class NotifyClientFactory : INotifyClientFactory
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        private readonly List<INotifyClient> clients = new List<INotifyClient>();

        /// <summary>
        /// 添加通知客户端
        /// </summary>
        /// <param name="client">通知客户端</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddClient(INotifyClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            this.clients.Add(client);
        }

        /// <summary>
        /// 执行通知
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async Task NotifyAsync(NotifyContent context)
        {
            foreach (var item in clients)
            {
                await item.NotifyAsync(context);
            }
        }
    }
}
