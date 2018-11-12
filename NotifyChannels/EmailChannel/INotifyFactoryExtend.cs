using Monitor.EmailChannel;
using System;

namespace Monitor
{
    /// <summary>
    /// 表示 INotifyFactory 扩展
    /// </summary>
    public static class INotifyFactoryExtend
    {
        /// <summary>
        /// 添加邮件通知 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">选项</param>
        public static void AddEmailNotifyChannel(this INotifyFactory factory, Action<EmailChannelOptions> options)
        {
            var opt = new EmailChannelOptions();
            options?.Invoke(opt);

            var channel = new EmailChanneler(opt);
            factory.AddChannel(channel);
        }
    }
}
