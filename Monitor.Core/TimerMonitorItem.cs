using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor.Core
{
    /// <summary>
    /// 表示基于定时器的监控对象
    /// </summary>
    public abstract class TimerMonitorItem : IMonitorItem
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// 选项
        /// </summary>
        private readonly TimerMonitorItemOptions options;

        /// <summary>
        /// 获取别名
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
        /// 基于定时器的监控对象
        /// </summary>
        /// <param name="options">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TimerMonitorItem(TimerMonitorItemOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.timer = new Timer(this.OnTimerTick, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="state"></param>
        private async void OnTimerTick(object state)
        {
            try
            {
                await this.CheckAsync();
            }
            catch (Exception ex)
            {
                var @event = this.OnException;
                @event?.Invoke(this, ex);
            }
            finally
            {
                this.timer.Change(this.options.Interval, Timeout.InfiniteTimeSpan);
            }
        }

        /// <summary>
        /// 检测对象，遇到问题则抛出异常
        /// </summary>
        /// <returns></returns>
        protected abstract Task CheckAsync();

        /// <summary>
        /// 启动
        /// </summary>
        void IMonitorItem.Start()
        {
            this.Start();
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void Start()
        {
            this.timer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// 释放资源 
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose()
        {
            this.timer.Dispose();
        }
    }
}
