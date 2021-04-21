using System.ComponentModel.DataAnnotations;

namespace BPMWebConsole.Models.ViewModels
{
    /// <summary>
    /// 登入密碼重置操作資料之集合類別
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// 重置之使用者登入密碼
        /// </summary>
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 再次確認重置之登入密碼
        /// </summary>
        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 收件者Email位址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 特定使用者有效重置密碼之操作代幣
        /// </summary>
        public string Token { get; set; }
    }
}
