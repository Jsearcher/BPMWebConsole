namespace EmailService
{
    /// <summary>
    /// Email服務之組態設定類別
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// 寄件者Email位址
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// SMTP伺服器主機名稱
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// 伺服器主機埠號(port)
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 伺服器主機之使用者帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 伺服器主機之使用者密碼
        /// </summary>
        public string Password { get; set; }
    }
}
