using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 表示通知工厂
    /// </summary>
    public interface INotifyFactory
    {
        /// <summary>
        /// 添加通知通道
        /// </summary>
        /// <param name="channel"></param>
        void AddChannel(INotifyChannel channel);

        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        Task NotifyAsync(NotifyContent context);
    }
}
