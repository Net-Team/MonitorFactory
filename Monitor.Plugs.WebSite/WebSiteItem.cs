using Monitor.Core;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// 表示网站监控对象
    /// </summary>
    public class WebSiteItem : TimerMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly WebSiteOptions options;

        /// <summary>
        /// 通知次数
        /// </summary>
        private int notifyTimes = 0;

        /// <summary>
        /// 网站监控对象
        /// </summary>
        static WebSiteItem()
        {
            HttpApiFactory.Add<IWebSiteApi>();
        }

        /// <summary>
        /// 网站监控对象
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebSiteItem(WebSiteOptions options)
            : base(options)
        {
            this.options = options;
        }

        /// <summary>
        /// 检测站点
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            var api = HttpApiFactory.Create<IWebSiteApi>();
            var token = new CancellationTokenSource(this.options.Timeout).Token;

            try
            {
                await api
                    .CheckAsync(this.options.Uri, token)
                    .Retry(this.options.Retry)
                    .WhenCatch<Exception>();
                this.notifyTimes = 0;
            }
            catch (Exception ex)
            {
                if (this.notifyTimes < this.options.MaxNotify)
                {
                    this.notifyTimes += 1;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 站点接口
        /// </summary>
        private interface IWebSiteApi : IHttpApi
        {
            /// <summary>
            /// 检测
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            [HttpGet]
            ITask<HttpResponseMessage> CheckAsync([Uri] Uri uri, CancellationToken token);
        }
    }
}
