using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MonitorApp
{
    public class Config
    {
        public Monitor.NotifyClients.Email.NotifyClientOptions MailOptions { get; set; }

        public Monitor.NotifyClients.Http.NotifyClientOptions HttpOptions { get; set; }


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
