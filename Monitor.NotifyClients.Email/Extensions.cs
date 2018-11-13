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
        public static INotifyClientFactory AddMailClient(this INotifyClientFactory factory, Action<NotifyClientOptions> options)
        {
            var opt = new NotifyClientOptions();
            options?.Invoke(opt);
            return factory.AddMailClient(opt);
        }

        /// <summary>
        /// 添加邮件通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        public static INotifyClientFactory AddMailClient(this INotifyClientFactory factory, NotifyClientOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var client = new NotifyClient(options);
            factory.AddClient(client);
            return factory;
        }
    }
}
