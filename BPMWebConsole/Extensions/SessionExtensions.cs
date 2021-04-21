using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BPMWebConsole.Extensions
{
    /// <summary>
    /// Session擴充方法類別
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// 設置Session存放內容
        /// </summary>
        /// <typeparam name="T">存放內容之型別</typeparam>
        /// <param name="session">Session名稱</param>
        /// <param name="key">對應的存放名稱</param>
        /// <param name="value">存放內容</param>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 取出Session存放內容
        /// </summary>
        /// <typeparam name="T">存放內容之型別</typeparam>
        /// <param name="session">Session名稱</param>
        /// <param name="key">對應的存放名稱</param>
        /// <returns>存放內容</returns>
        public static T GetObject<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
