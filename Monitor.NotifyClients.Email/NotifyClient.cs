using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Monitor.NotifyClients.Email
{
    /// <summary>
    /// 表示邮件通知通道
    /// </summary>
    public class NotifyClient : INotifyClient
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly NotifyClientOptions opt;

        /// <summary>
        /// 邮件通知
        /// </summary>
        /// <param name="opt">选项</param>
        public NotifyClient(NotifyClientOptions opt)
        {
            this.opt = opt;
        }

        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public async Task NotifyAsync(NotifyContent context)
        {
            await this.SendEmailAsync(context.Title, context.Message);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        private async Task SendEmailAsync(string title, string body)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(this.opt.SenderAccout),
                Subject = title,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = false,
            };

            foreach (var item in this.opt.TargetEmails.Distinct())
            {
                if (string.IsNullOrEmpty(item) == false && Regex.IsMatch(item, @"^\w+(\.\w*)*@\w+\.\w+$"))
                {
                    msg.To.Add(item);
                }
            }

            if (msg.To.Count == 0)
            {
                return;
            }

            using (var client = new SmtpClient())
            {
                client.Credentials = new NetworkCredential(this.opt.SenderAccout, this.opt.SenderPassword);
                client.Port = this.opt.Port;
                client.Host = this.opt.Smtp;
                client.EnableSsl = this.opt.SSL;
                await client.SendMailAsync(msg);
            }
        }
    }
}
