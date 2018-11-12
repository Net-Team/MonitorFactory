namespace Monitor
{
    /// <summary>
    /// 表示通知内容接口
    /// </summary>
    public class NotifyContent
    {
        /// <summary>
        /// 获取或设置通知标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置通知内容
        /// </summary>
        public string Message { get; set; }
    }
}
