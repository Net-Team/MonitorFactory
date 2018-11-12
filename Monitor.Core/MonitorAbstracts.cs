using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Core
{
    /// <summary>
    /// 监控插件基类
    /// </summary>
    public class MonitorAbstracts
    {

#if NET451
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        public static readonly Task CompletedTask = Task.FromResult<object>(null);
#else
        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        public static readonly Task CompletedTask = Task.CompletedTask;
#endif

        /// <summary>
        /// 加载json配置
        /// json文件为插件文件名.json
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        public TConfig LoadJsonConfig<TConfig>()
        {
            var file = Path.ChangeExtension(this.GetType().Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                return default(TConfig);
            }

            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<TConfig>(json);
        }
    }
}
