using System.Threading.Tasks;

namespace EmailService
{
    /// <summary>
    /// Email信件傳送器實作介面
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// 傳送Email信件方法
        /// </summary>
        /// <param name="message">信件內容訊息物件</param>
        void SendEmail(Message message);

        /// <summary>
        /// 非同步方式傳送Email信件方法
        /// </summary>
        /// <param name="message">信件內容訊息物件</param>
        /// <returns>非同步處理執行緒</returns>
        Task SendEmailAsync(Message message);
    }
}
