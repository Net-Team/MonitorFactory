using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Plugs.Processor
{
    /// <summary>
    /// 表示处理器配置信息
    /// </summary>
    public class ProcessorConfig
    {
        /// <summary>
        /// 处理器监控配置集合
        /// </summary>
        public ProcessorOptions[] Options { get; set; }
    }
}
