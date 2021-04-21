using Microsoft.AspNetCore.Identity;

namespace BPMWebConsole.Models.Entities
{
    /// <summary>
    /// 使用者資料類別(繼承 <see cref="IdentityUser"/> 之屬性資料)
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string LastName { get; set; }
    }
}
