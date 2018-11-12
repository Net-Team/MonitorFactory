using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Core
{
    /// <summary>
    /// 表示监控到的异常基类
    /// 此异常用于通知
    /// </summary>
    public class MonitorException : Exception
    {

        public string Alias { get; private set; }

        /// <summary>
        /// 监控到的异常
        /// </summary>
        /// <param name="inner">内部异常</param>
        /// <param name="alias"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MonitorException(Exception inner, string alias) :
            base(inner.Message, inner)
        {
            this.Alias = alias;
        }
    }
}
