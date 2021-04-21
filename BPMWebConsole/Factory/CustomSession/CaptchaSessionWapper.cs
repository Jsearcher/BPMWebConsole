using BPMWebConsole.Extensions;
using BPMWebConsole.Models.ConfigScript;
using Microsoft.AspNetCore.Http;

namespace BPMWebConsole.Factory.CustomSession
{
    /// <summary>
    /// Session強型別包裝類別
    /// </summary>
    public class CaptchaSessionWapper : ISessionWapper<string>
    {
        #region =====[Private] Variable=====

        /// <summary>
        /// Session key
        /// </summary>
        private static readonly string _key = WebConfig.WebPropertySetting.Instance().SessionKey.CaptchaImg;

        /// <summary>
        /// HTTP Context接收器
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Session物件
        /// </summary>
        private ISession Session { get => _httpContextAccessor.HttpContext.Session; }

        #endregion
        #region =====[Public] Property=====

        /// <summary>
        /// 目標模型物件
        /// </summary>
        public string Model
        {
            get => Session.GetObject<string>(_key);
            set => Session.SetObject(_key, value);
        }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="httpContextAccessor">HTTP Context接收器</param>
        public CaptchaSessionWapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion
    }
}
