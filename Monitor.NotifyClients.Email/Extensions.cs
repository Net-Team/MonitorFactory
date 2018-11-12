using Monitor.NotifyClients.Email;
using System;

namespace Monitor
{
    /// <summary>
    /// 表示 INotifyFactory 扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 添加邮件通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        public static INotifyClientFactory AddClient(this INotifyClientFactory factory, Action<NotifyClientOptions> options)
        {
            var opt = new NotifyClientOptions();
            options?.Invoke(opt);

            var channel = new NotifyClient(opt);
            factory.AddClient(channel);

            return factory;
        }
    }
}
