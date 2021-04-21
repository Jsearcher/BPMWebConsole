using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace EmailService
{
    /// <summary>
    /// 信件內容訊息類別
    /// </summary>
    public class Message
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 收件者Email位址集合列表
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// 信件主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 信件內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 信件附件檔案
        /// </summary>
        public IFormFileCollection Attachments { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="to">收件者Email位址集合列表</param>
        /// <param name="subject">信件主旨</param>
        /// <param name="content">信件內容</param>
        /// <param name="attachments">信件附件檔案</param>
        public Message(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }

        #endregion
    }
}
