namespace Monitor.Plugs.Process
{
    /// <summary>
    /// 表示进程监控选项
    /// </summary>
    public class ProcessOptions
    {
        /// <summary>
        /// 获取或设置进程别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 获取或设置进程的文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 获取或设置启动参数字符串
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 获取或设置进程的工作目录
        /// </summary>
        public string WorkingDirectory { get; set; }
    }
}
