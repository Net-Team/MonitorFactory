using System.Collections.Generic;

namespace Monitor.NotifyClients.Email
{
    /// <summary>
    /// 表示邮件通知选项
    /// </summary>
    public class NotifyClientOptions
    {
        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string Smtp { get; set; }

        /// <summary>
        /// 或取或设置服务器使用的mail端口
        /// 默认为25
        /// </summary>
        public int Port => 25;

        /// <summary>
        /// 或取或设置是否使用SSL
        /// 默认为false
        /// </summary>
        public bool SSL => false;

        /// <summary>
        /// 或取或设置发送者的邮箱账号
        /// </summary>
        public string SenderAccout { get; set; }

        /// <summary>
        /// 或取或设置发送者的邮箱密码
        /// </summary>
        public string SenderPassword { get; set; }

        /// <summary>
        /// 或取接收者邮箱地址列表
        /// </summary>
        public List<string> TargetEmails { get; } = new List<string>(); 
    }
}
