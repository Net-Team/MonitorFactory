using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Monitor.Plugs.Processor
{
    public static class ProcessorInfo
    {
        /// <summary>
        /// Windows 性能数器组件
        /// </summary>
        private static PerformanceCounter performanceCounter = new PerformanceCounter();

        /// <summary>
        /// Cpu 数量
        /// </summary>
        private static int CpuCount = Environment.ProcessorCount;

        /// <summary>
        /// Cpu 使用率
        /// </summary>
        public static float CpuUsage
        {
            get
            {
                return performanceCounter.NextValue();
            }
        }

        /// <summary>
        /// 设置性能组件
        /// </summary>
        static ProcessorInfo()
        {
            performanceCounter.CategoryName = "Processor Information";
            performanceCounter.CounterName = "% Processor Time";
            performanceCounter.InstanceName = "_Total";
            performanceCounter.MachineName = ".";
            performanceCounter.ReadOnly = true;
            performanceCounter.NextValue();
        }
    }
}
