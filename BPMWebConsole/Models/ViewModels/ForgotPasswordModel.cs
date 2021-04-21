using System.ComponentModel.DataAnnotations;

namespace BPMWebConsole.Models.ViewModels
{
    /// <summary>
    /// 忘記密碼資料集合類別
    /// </summary>
    public class ForgotPasswordModel
    {
        /// <summary>
        /// 收件者Email位址
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
