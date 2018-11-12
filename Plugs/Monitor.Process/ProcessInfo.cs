using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Monitor.Process
{
    /// <summary>
    /// 应用进程描述信息
    /// </summary>
    public class ProcessInfo : IMonitor
    {
        /// <summary>
        /// 别名
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
        /// 构建应用程序信息
        /// </summary>
        /// <param name="filePath">进程的文件路径</param>
        /// <param name="arguments">启动参数字符串</param>
        /// <param name="workingDirectory">工作路径</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProcessInfo(string filePath, string arguments = null, string workingDirectory = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(filePath);
            }

            this.FilePath = Path.GetFullPath(filePath);
            this.Arguments = arguments;
            this.WorkingDirectory = workingDirectory ?? Path.GetDirectoryName(this.FilePath);
        }

        /// <summary>
        /// 异常触发事件
        /// </summary>
        public event Action<IMonitor, Exception> OnException;

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
            try
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
            catch (Exception ex)
            {
                this.OnException.Invoke(this, ex);
            }
        }
    }
}
