using Lib.Log;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    /// <summary>
    /// Email信件傳送器類別
    /// </summary>
    public class EmailSender : IEmailSender
    {
        #region =====[Private] Variable=====

        /// <summary>
        /// Email服務組態設定物件
        /// </summary>
        private readonly EmailConfiguration _emailConfig;

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="emailConfig">Email服務組態設定物件</param>
        public EmailSender(EmailConfiguration emailConfig)
        {
            this._emailConfig = emailConfig;
        }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 傳送Email信件方法
        /// </summary>
        /// <param name="message">信件內容訊息物件</param>
        public void SendEmail(Message message)
        {
            Send(CreateEmailMessage(message));
        }

        /// <summary>
        /// 非同步方式傳送Email信件方法
        /// </summary>
        /// <param name="message">信件內容訊息物件</param>
        /// <returns>非同步處理執行緒</returns>
        public async Task SendEmailAsync(Message message)
        {
            await SendAsync(CreateEmailMessage(message));
        }

        #endregion

        #region =====[Private] Function=====

        /// <summary>
        /// 新增電子郵件
        /// </summary>
        /// <param name="message">信件內容訊息物件</param>
        /// <returns>MIME格式之Email</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            BodyBuilder bodyBuilder = new BodyBuilder { HtmlBody = $"<h2 style='color:red;'>{message.Content}</h2>" };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (IFormFile attachment in message.Attachments)
                {
                    using MemoryStream ms = new MemoryStream();
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            MimeMessage emailMessage = new MimeMessage
            {
                Subject = message.Subject,
                //Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content }
                //Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h2 style='color:red;'>{message.Content}</h2>" }
                Body = bodyBuilder.ToMessageBody()
            };
            emailMessage.From.Add(new MailboxAddress(this._emailConfig.From));
            emailMessage.To.AddRange(message.To);

            return emailMessage;
        }

        /// <summary>
        /// 傳送MIME格式Email
        /// </summary>
        /// <param name="mailMessage">MIME格式之Email</param>
        private void Send(MimeMessage mailMessage)
        {
            using SmtpClient client = new SmtpClient();
            try
            {
                client.Connect(this._emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(this._emailConfig.UserName, this._emailConfig.Password);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("EmailSender", ex);
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        /// <summary>
        /// 非同步方式傳送MIME格式Email
        /// </summary>
        /// <param name="mailMessage">MIME格式之Email</param>
        /// <returns>非同步處理執行緒</returns>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using SmtpClient client = new SmtpClient();
            try
            {
                await client.ConnectAsync(this._emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(this._emailConfig.UserName, this._emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                ErrorLog.Log("EmailSender", ex);
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        #endregion
    }
}
