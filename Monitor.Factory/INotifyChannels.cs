using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 表示通知接口
    /// </summary>
    public interface INotifyChannel
    {
        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        Task NotifyAsync(NotifyContent context);
    }
}
