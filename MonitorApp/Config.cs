using Monitor.NotifyClients.Email;
using Monitor.NotifyClients.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MonitorApp
{
    /// <summary>
    /// 表示配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 邮件通知选项
        /// </summary>
        public MailNotifyClientOptions MailOptions { get; set; }

        /// <summary>
        /// http通知选项
        /// </summary>
        public HttpNotifyClientOptions HttpOptions { get; set; }


        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static Config Load()
        {
            var file = Path.ChangeExtension(typeof(Config).Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                throw new FileNotFoundException("找不到配置文件", file);
            }

            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}
