using Monitor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Plugs.Telnet
{
    /// <summary>
    /// Telnet 监控对象
    /// </summary>
    public class TelnetItem : TimerMonitorItem
    {
        /// <summary>
        /// Telnet 选项
        /// </summary>
        private readonly TelnetOptions options;

        /// <summary>
        /// 第一次通知时间
        /// </summary>
        private DateTime firstNotifyTime = DateTime.MinValue;

        /// <summary>
        /// 上一次通知时间戳
        /// </summary>
        private TimeSpan lastTimeSpan = TimeSpan.Zero;

        /// <summary>
        /// 构造 Telnet 监控对象
        /// </summary>
        /// <param name="options"></param>
        public TelnetItem(TelnetOptions options)
            : base(options)
        {
            if (options.NotifyTimeSpan.Length == 0)
            {
                options.NotifyTimeSpan = new TimeSpan[] { options.Interval };
            }
            this.options = options;
        }

        /// <summary>
        /// 执行检测
        /// </summary>
        /// <returns></returns>
        protected override async Task CheckAsync()
        {
            var tcpClient = new TcpClient();
            try
            {
                await tcpClient.ConnectAsync(this.options.Host, this.options.Port);
                tcpClient.Close();
                firstNotifyTime = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                if (this.firstNotifyTime == DateTime.MinValue)
                {
                    this.firstNotifyTime = DateTime.Now;
                    throw ex;
                }

                var curTimeSpan = DateTime.Now.Subtract(firstNotifyTime);

                //当配置1个或多个间隔时间的时候
                foreach (var target in this.options.NotifyTimeSpan.OrderBy(item => item))
                {
                    if (curTimeSpan > target && target > lastTimeSpan)
                    {
                        lastTimeSpan = curTimeSpan;
                        throw ex;
                    }
                }
            }
        }
    }
}
