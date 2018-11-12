using Monitor.Core;
using System;
using System.Net.Http;
using System.Threading;

namespace Monitor.Plugs.WebSite
{
    /// <summary>
    /// 表示网站监控对象
    /// </summary>
    public class WebSiteItem : IMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly WebSiteOptions options;

        /// <summary>
        /// 定时器
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// 网站别名
        /// </summary>
        public string Alias
        {
            get => this.options.Alias;
        }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<IMonitorItem, Exception> OnException;

        /// <summary>
        /// 网站监控对象
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebSiteItem(WebSiteOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.timer = new Timer(this.OnTimerTick, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 检测网站
        /// </summary>
        /// <param name="state"></param>
        private async void OnTimerTick(object state)
        {
            try
            {
                using (var client = new HttpClient { Timeout = this.options.Timeout })
                {
                    var response = await client.GetAsync(this.options.Uri);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                var @event = this.OnException;
                if (@event != null)
                {
                    @event.Invoke(this, ex);
                }
            }
            finally
            {
                this.timer.Change(TimeSpan.Zero, this.options.Interval);
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            this.timer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}
