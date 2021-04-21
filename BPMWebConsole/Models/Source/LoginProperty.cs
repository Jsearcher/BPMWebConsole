using System.ComponentModel.DataAnnotations;

namespace BPMWebConsole.Models.Source
{
    /// <summary>
    /// 使用者登入屬性資料類別
    /// </summary>
    public class LoginProperty
    {
        /// <summary>
        /// 使用者登入帳號
        /// </summary>
        [Required, EmailAddress]
        [Display(Name = nameof(Account))]
        public string Account { get; set; }

        /// <summary>
        /// 使用者登入密碼
        /// </summary>
        [Required, DataType(DataType.Password)]
        [Display(Name = nameof(Password))]
        public string Password { get; set; }

        /// <summary>
        /// 記住登入帳號對應的密碼
        /// </summary>
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// 圖形驗證碼字串
        /// </summary>
        [Required]
        public string CaptchaString { get; set; }
    }
}
