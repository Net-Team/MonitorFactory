using Monitor.NotifyClients.Http;
using System;

namespace Monitor
{
    /// <summary>
    /// 表示 INotifyFactory 扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 添加Http通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        public static INotifyClientFactory AddHttpClient(this INotifyClientFactory factory, Action<NotifyClientOptions> options)
        {
            var opt = new NotifyClientOptions();
            options?.Invoke(opt);
            return factory.AddHttpClient(opt);
        }

        /// <summary>
        /// 添加Http通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static INotifyClientFactory AddHttpClient(this INotifyClientFactory factory, NotifyClientOptions options)
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
