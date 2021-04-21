using System.ComponentModel.DataAnnotations;

namespace BPMWebConsole.Models.ViewModels
{
    /// <summary>
    /// 使用者註冊資料集合類別
    /// </summary>
    public class UserRegistrationModel
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 註冊電子郵件信箱
        /// </summary>
        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 註冊之登入密碼
        /// </summary>
        [Required(ErrorMessage = "Password is required"), DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 再次確認之登入密碼
        /// </summary>
        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        #endregion
    }
}
