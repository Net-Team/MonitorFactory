using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.Telnet
{
    /// <summary>
    /// Telnet 监控插件配置
    /// </summary>
    class TelnetPlugConfig
    {
        /// <summary>
        /// Telnet 监控对象集合
        /// </summary>
        public TelnetOptions[] Options { get; set; }
    }
}
