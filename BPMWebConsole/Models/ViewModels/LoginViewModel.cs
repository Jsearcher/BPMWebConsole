using BPMWebConsole.Models.Source;

namespace BPMWebConsole.Models.ViewModels
{
    /// <summary>
    /// 登入頁面資料集合類別
    /// </summary>
    public class LoginViewModel
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 使用者登入屬性資料集合
        /// </summary>
        public LoginProperty LoginProp { get; set; }

        /// <summary>
        /// 登入驗證成功後導向之頁面網址
        /// </summary>
        public string ReturnUrl { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        public LoginViewModel() { }

        #endregion
    }
}
