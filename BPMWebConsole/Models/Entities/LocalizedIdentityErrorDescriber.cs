using Microsoft.AspNetCore.Identity;

namespace BPMWebConsole.Models.Entities
{
    /// <summary>
    /// <c>Identity</c> 錯誤說明類別
    /// </summary>
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// 使用者資料 <c>Email</c> 已存在之錯誤說明
        /// </summary>
        /// <param name="email">Email位址</param>
        /// <returns><see cref="IdentityError"/> 錯誤說明物件</returns>
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "輸入的Email已被使用"
            };
        }

        /// <summary>
        /// 使用者資料 <c>UserName</c> 已存在之錯誤說明
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <returns><see cref="IdentityError"/> 錯誤說明物件</returns>
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "輸入的UserName已被使用"
            };
        }

        /// <summary>
        /// 使用者資料 <c>Email</c> 格式無效之錯誤說明
        /// </summary>
        /// <param name="email">Email位址</param>
        /// <returns><see cref="IdentityError"/> 錯誤說明物件</returns>
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "輸入的Email格式錯誤"
            };
        }

        /// <summary>
        /// 使用者資料 <c>UserName</c> 格式無效之錯誤說明
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <returns><see cref="IdentityError"/> 錯誤說明物件</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "輸入的UserName格式錯誤"
            };
        }
    }
}
