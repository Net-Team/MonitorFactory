using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 定义通知客户端工厂的接口
    /// </summary>
    public interface INotifyClientFactory
    {
        /// <summary>
        /// 添加通知客户端
        /// </summary>
        /// <param name="client">通知客户端</param>
        void AddClient(INotifyClient client);

        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        Task NotifyAsync(NotifyContent context);
    }
}
