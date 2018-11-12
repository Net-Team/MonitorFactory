using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClient;

namespace Monitor.HttpChannel
{
    /// <summary>
    /// 表示 Http 通知通道
    /// </summary>
    public class HttpChanneler : INotifyChannel
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly HttpChannelOptions opt;

        /// <summary>
        /// 异常通知接口
        /// </summary>
        private readonly IHttpNotifyClient httpNotifyClient = HttpApiClient.Create<IHttpNotifyClient>();

        /// <summary>
        /// Http异常通知通道
        /// </summary>
        /// <param name="opt">选项</param>
        public HttpChanneler(HttpChannelOptions opt)
        {
            this.opt = opt;
        }

        /// <summary>
        /// Http 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public async Task NotifyAsync(NotifyContent context)
        {
            var httpContent = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Title",context.Title),
                new KeyValuePair<string, string>("Message",context.Message)
            };


            await this.httpNotifyClient
                .SendNotifyAsync(this.opt.Uri, this.opt.Header, httpContent);
        }
    }
}
