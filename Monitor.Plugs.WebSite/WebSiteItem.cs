using Microsoft.Extensions.Logging;
using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.Contexts;
using WebApiClient.Parameterables;

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
        /// 结果匹配
        /// </summary>
        private readonly ResultMatch resultMatch;

        /// <summary>
        /// httpApi工厂
        /// </summary>
        private readonly HttpApiFactory<IWebSiteApi> httpApiFactory;


        /// <summary>
        /// 通知次数
        /// </summary>
        private int notifyTimes = 0;

        /// <summary>
        /// 网站监控对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebSiteItem(WebSiteOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            this.options = options;
            this.resultMatch = new ResultMatch(options.ResultMatch);
            this.httpApiFactory = new HttpApiFactory<IWebSiteApi>().ConfigureHttpApiConfig(c =>
            {
                c.LoggerFactory = loggerFactory;
            });
        }

        /// <summary>
        /// 检测站点
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            try
            {
                var method = new Method(this.options.Method);
                if (method == HttpMethod.Get || method == HttpMethod.Head)
                {
                    await this.httpApiFactory.CreateHttpApi()
                        .CheckAsync(this.options.Uri, method, this.options.Header, this.options.Timeout)
                        .Retry(this.options.Retry)
                        .WhenCatch<Exception>()
                        .WhenResult(html => this.resultMatch.IsMatch(html) == false);
                }
                else
                {
                    var content = new StringContent(this.options.Content, Encoding.UTF8, this.options.ContentType);
                    await this.httpApiFactory.CreateHttpApi()
                        .CheckAsync(this.options.Uri, method, this.options.Header, content, this.options.Timeout)
                        .Retry(this.options.Retry)
                        .WhenCatch<Exception>()
                        .WhenResult(html => this.resultMatch.IsMatch(html) == false);
                }

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
        [TraceFilter]
        private interface IWebSiteApi : IHttpApi
        {
            /// <summary>
            /// 检测站点
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="uri">uri</param>
            /// <param name="header">请求头</param>
            /// <param name="timeout">超时时间</param>
            /// <returns></returns>
            ITask<string> CheckAsync([Uri] Uri uri, Method method, [Headers] IEnumerable<KeyValuePair<string, string>> header, Timeout timeout);

            /// <summary>
            /// 检测站点
            /// </summary>
            /// <param name="method">请求方法</param>
            /// <param name="uri">uri</param>
            /// <param name="header">请求头</param>
            /// <param name="content">请求内容</param>
            /// <param name="timeout">超时时间</param>
            /// <returns></returns>
            ITask<string> CheckAsync([Uri] Uri uri, Method method, [Headers] IEnumerable<KeyValuePair<string, string>> header, HttpContent content, Timeout timeout);
        }

        /// <summary>
        /// 表示请求方法
        /// </summary>
        private class Method : HttpMethod, IApiParameterable
        {
            /// <summary>
            /// 请求方法
            /// </summary>
            /// <param name="method"></param>
            public Method(string method)
                : base(method)
            {
            }

            public Task BeforeRequestAsync(ApiActionContext context, ApiParameterDescriptor parameter)
            {
                context.RequestMessage.Method = this;
                return Task.FromResult<object>(null);
            }
        }

        /// <summary>
        /// 表示结果匹配
        /// </summary>
        private class ResultMatch
        {
            /// <summary>
            /// 正则
            /// </summary>
            private readonly Regex regex;

            /// <summary>
            /// 结果匹配
            /// </summary>
            /// <param name="pattern">匹配规则，*代表任意</param>
            public ResultMatch(string pattern)
            {
                if (string.IsNullOrEmpty(pattern) == true)
                {
                    this.regex = new Regex(".*");
                }
                else
                {
                    this.regex = new Regex(Regex.Escape(pattern).Replace("\\*", ".*"), RegexOptions.IgnoreCase);
                }
            }

            /// <summary>
            /// 是否与规则匹配
            /// </summary>
            /// <param name="input">输入项</param>
            /// <returns></returns>
            public bool IsMatch(string input)
            {
                return this.regex.IsMatch(input);
            }
        }
    }
}
