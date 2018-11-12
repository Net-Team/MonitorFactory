using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// 表示监控对象接口
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// 获取别名
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        event Action<IMonitor, Exception> OnException;
    }
}
