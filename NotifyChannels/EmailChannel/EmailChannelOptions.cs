using System.Collections.Generic;

namespace Monitor.EmailChannel
{
    /// <summary>
    /// 表示邮件通知选项
    /// </summary>
    public class EmailChannelOptions
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

        ///// <summary>
        ///// 邮件参数 
        /////或取或设置 邮件标题参数委托
        ///// </summary>
        //public Func<NotifyContext, string> Title { get; set; } = ctx => ctx.ToTitle();

        ///// <summary>
        ///// 邮件参数 
        ///// 或取或设置邮件内容委托
        ///// </summary>
        //public Func<NotifyContext, string> Message { get; set; } = ctx => ctx.ToMessage();
    }
}
