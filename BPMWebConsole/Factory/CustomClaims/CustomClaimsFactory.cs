using BPMWebConsole.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BPMWebConsole.Factory.CustomClaims
{
    /// <summary>
    /// 提供使用者宣告準則之方法類別
    /// </summary>
    public class CustomClaimsFactory : UserClaimsPrincipalFactory<User>
    {
        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userManager">管理所儲存之使用者資料實體的方法API</param>
        /// <param name="optionsAccessor">Identity中所有可使用之組態設定選項</param>
        public CustomClaimsFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor) { }

        #endregion

        #region =====[Protected|Override] Method=====

        /// <summary>
        /// 異步產生使用者宣告準則
        /// </summary>
        /// <param name="user">使用者資料物件</param>
        /// <returns>身分宣告物件集合</returns>
        /// <remarks>在Cookie中加入宣告的使用者資訊(姓名、角色)</remarks>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullName", $"{user.FirstName} {user.LastName}"));

            IList<string> roles = await UserManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }

        #endregion
    }
}
