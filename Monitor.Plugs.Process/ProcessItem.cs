using Monitor.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Monitor.Plugs.Process
{
    /// <summary>
    /// 表示进程监控项
    /// </summary>
    public class ProcessItem : IMonitorItem
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly ProcessOptions options;

        /// <summary>
        /// 关联的进程
        /// </summary>
        private System.Diagnostics.Process process;

        /// <summary>
        /// 进程别名
        /// </summary>
        public string Alias
        {
            get => this.options.Alias;
        }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        public event Action<IMonitorItem, Exception> OnException;

        /// <summary>
        /// 进程监控
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public ProcessItem(ProcessOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (File.Exists(options.FilePath) == false)
            {
                throw new FileNotFoundException("找不到进程文件", options.FilePath);
            }

            this.options = options;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            var processName = Path.GetFileNameWithoutExtension(this.options.FilePath);
            this.process = System.Diagnostics.Process.GetProcessesByName(processName)?.FirstOrDefault();

            if (this.process == null)
            {
                this.process = this.CreateProcess();
            }

            this.process.EnableRaisingEvents = true;
            this.process.Exited += Process_Exited;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.process != null)
            {
                this.process.Exited -= Process_Exited;
            }
        }

        /// <summary>
        /// 进程退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Exited(object sender, EventArgs e)
        {
            var @event = this.OnException;
            if (@event != null)
            {
                try
                {
                    this.process = this.CreateProcess();
                    this.process.EnableRaisingEvents = true;
                    this.process.Exited += Process_Exited;

                    var ex = new Exception($"发现进程{this.options.FilePath}退出，重启进程成功！");
                    @event.Invoke(this, ex);
                }
                catch (Exception ex)
                {
                    var exception = new Exception($"发现进程{this.options.FilePath}退出，重启进程失败！", ex);
                    @event.Invoke(this, exception);
                }
            }
        }

        /// <summary>
        /// 创建进程
        /// </summary>
        /// <returns></returns>
        private System.Diagnostics.Process CreateProcess()
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = this.options.Arguments,
                FileName = this.options.FilePath,
                WorkingDirectory = this.options.WorkingDirectory
            };
            return System.Diagnostics.Process.Start(startInfo);
        }
    }
}
