using Monitor.Core;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
        /// 网站监控对象
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebSiteItem(WebSiteOptions options)
            : base(options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 检测站点
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            using (var client = new HttpClient { Timeout = this.options.Timeout })
            {
                var response = await client.GetAsync(this.options.Uri);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
