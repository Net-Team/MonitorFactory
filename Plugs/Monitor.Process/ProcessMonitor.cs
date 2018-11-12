using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Monitor.Process
{
    /// <summary>
    /// 程序监控
    /// </summary>
    public class ProcessMonitor : IMonitor
    {
        /// <summary>
        /// 程序别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 获取进程的文件路径
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 获取启动参数字符串
        /// </summary>
        public string Arguments { get; private set; }

        /// <summary>
        /// 获取进程的工作目录
        /// </summary>
        public string WorkingDirectory { get; private set; }

        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        public event Action<IMonitor, Exception> OnException;

        /// <summary>
        /// 程序监控
        /// </summary>
        public ProcessMonitor()
        {
        }

        /// <summary>
        /// 检测进程是否已运行
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            var processName = Path.GetFileNameWithoutExtension(this.FilePath);
            var process = System.Diagnostics.Process.GetProcessesByName(processName)?.FirstOrDefault();
            if (process == null)
            {
                return false;
            }
            return string.Equals(process.MainModule.FileName, this.FilePath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 启动进程
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public void Start()
        {

            while (true)
            {
                try
                {
                    if (this.IsRunning() == false)
                    {
                        this.ProcessStart();
                    }
                }
                catch (Exception ex)
                {
                    this.OnException.Invoke(this, ex);
                }
                Thread.Sleep(5000);
            }
        }

        public void ProcessStart()
        {
            if (File.Exists(this.FilePath) == false)
            {
                throw new FileNotFoundException(this.FilePath);
            }

            var startInfo = new ProcessStartInfo
            {
                Arguments = this.Arguments,
                FileName = this.FilePath,
                WorkingDirectory = this.WorkingDirectory
            };
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}
