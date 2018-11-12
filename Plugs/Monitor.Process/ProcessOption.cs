namespace Monitor.Process
{
    /// <summary>
    /// 表示程序监控选项
    /// </summary>
    public class ProcessOption : IPlugOption<ProcessInfo>
    {
        /// <summary>
        /// 轮循时间间隔
        /// 单位秒
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProcessInfo[] Monitors { get; set; }
    }
}
