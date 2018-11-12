using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace Monitor.HttpChannel
{
    /// <summary>
    /// Http 异常通知接口
    /// </summary>
    public interface IHttpNotifyClient : IDisposable
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
            [Url] Uri url,
            [Headers] IEnumerable<KeyValuePair<string, string>> header,
            [FormContent] IEnumerable<KeyValuePair<string, string>> content);
    }
}
