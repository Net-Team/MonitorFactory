using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 定义通知客户端的接口
    /// </summary>
    public interface INotifyClient
    {
        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        Task NotifyAsync(NotifyContent context);
    }
}
