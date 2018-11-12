using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace Monitor.NotifyClients.Http
{
    /// <summary>
    /// 表示Http通知通道
    /// </summary>
    public class NotifyClient : INotifyClient
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly NotifyClientOptions opt;

        /// <summary>
        /// Http通知通道
        /// </summary>
        static NotifyClient()
        {
            HttpApiFactory.Add<IHttpNotifyApi>();
        }

        /// <summary>
        /// Http通知通道
        /// </summary>
        /// <param name="opt">选项</param>
        public NotifyClient(NotifyClientOptions opt)
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

            await HttpApiFactory.Create<IHttpNotifyApi>()
                .SendNotifyAsync(this.opt.Uri, this.opt.Header, httpContent);
        }


        /// <summary>
        /// Http异常通知接口
        /// </summary>
        private interface IHttpNotifyApi : IHttpApi
        {
            /// <summary>
            /// 发送Http请求
            /// </summary>
            /// <param name="url"></param>
            /// <param name="header">请求头</param>
            /// <param name="content">内容</param>
            /// <returns></returns>
            [HttpPost]
            ITask<HttpResponseMessage> SendNotifyAsync(
                [Uri] Uri url,
                [Headers] IEnumerable<KeyValuePair<string, string>> header,
                [FormContent] IEnumerable<KeyValuePair<string, string>> content);
        }
    }
}
