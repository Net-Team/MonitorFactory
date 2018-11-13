using Monitor.Core;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Monitor.Plugs.Service
{
    /// <summary>
    /// 表示服务对象
    /// </summary>
    public class ServiceItem : TimerMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly ServiceOptions options;

        /// <summary>
        /// 关联的服务
        /// </summary>
        private readonly ServiceController service;

        /// <summary>
        /// 服务对象
        /// </summary>
        /// <param name="options">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ServiceItem(ServiceOptions options)
            : base(options)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                throw new ArgumentNullException(nameof(options.Name));
            }

            this.options = options;
            this.service = new ServiceController(options.Name);
        }

        /// <summary>
        /// 检测对象，遇到问题则抛出异常
        /// </summary>
        /// <returns></returns>
        protected override Task CheckAsync()
        {
            try
            {
                this.service.Refresh();
                if (this.service.Status != ServiceControllerStatus.Stopped)
                {
                    return this.CompletedTask;
                }

                this.service.Start();
            }
            catch (Exception ex)
            {
                throw new Exception($"发现服务{this.Alias}停止，重启服务失败！", ex);
            }

            throw new Exception($"发现服务{this.Alias}停止，重启服务成功！");
        }
    }
}
