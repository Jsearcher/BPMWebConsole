using Lib.Misc.Property;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BPMWebConsole.Models.ConfigScript
{
    /// <summary>
    /// WEB應用程式中 <c>appsettings.json</c> 組態設定("WebPropSetting")之處理類別
    /// </summary>
    public class WebConfig : JsonPropertyBase
    {
        #region =====[Public] Class for Reflecting the Property Sections and Its Elements=====

        #region =====組態設定區段之依賴類別組 (For DI)=====
        /// <summary>
        /// DB相關組態設定參數類別
        /// </summary>
        public class DBServer
        {
            /// <summary>
            /// DB連接參數
            /// </summary>
            public string ServerDB { get; set; }
            /// <summary>
            /// BPM Server DB連接參數
            /// </summary>
            public string BPMDB { get; set; }
        }
        /// <summary>
        /// 其他基本組態設定參數類別
        /// </summary>
        public class Basic
        {
            /// <summary>
            /// 程式執行模式
            /// </summary>
            /// <remarks>
            /// <para>0: 表示為測試模式</para>
            /// <para>1: 表示為正式運行模式</para>
            /// </remarks>
            public int Mode { get; set; }
            /// <summary>
            /// 測試模式運行下使用之測試日期，格式為"yyyy-MM-dd"
            /// </summary>
            public string TestDate { get; set; }
            /// <summary>
            /// 檢查航空公司MQ連線處理之時間間隔(秒)
            /// </summary>
            public int MQCheckPeriod { get; set; }
        }
        /// <summary>
        /// 記錄暫存組態設定參數類別
        /// </summary>
        public class TempRecord
        {
            /// <summary>
            /// 各航空公司MQ連線之處理剩餘數(上一次)
            /// </summary>
            public string AirlineMQ { get; set; }
        }
        /// <summary>
        /// 組態設定參數集合類別
        /// </summary>
        public class WebPropSetting
        {
            /// <summary>
            /// DB相關組態設定參數類別物件
            /// </summary>
            public DBServer DBServer { get; set; }
            /// <summary>
            /// 其他基本組態設定參數類別物件
            /// </summary>
            public Basic Basic { get; set; }
            /// <summary>
            /// 記錄暫存組態設定參數類別物件
            /// </summary>
            public TempRecord TempRecord { get; set; }
        }
        /// <summary>
        /// 組態設定參數根集合類別
        /// </summary>
        public class WebSetting
        {
            /// <summary>
            /// 組態設定參數集合類別
            /// </summary>
            public WebPropSetting WebPropSetting { get; set; }
        }
        #endregion

        /// <summary>
        /// 依賴類別注入服務之應用類別，取得指定之組態設定檔內容(WebSetting)
        /// </summary>
        /// <remarks>
        /// <para>泛型<c>T</c>之對應之組態設定區段的依賴類別，使用<c>WebPropSetting</c></para>
        /// <para><c>properties</c>參數依 <c>WebPropSetting</c> 依賴類別存取目前組態設定值</para>
        /// </remarks>
        public class WebPropSettingDI : AppSettingsDI<WebPropSetting>
        {
            #region =====[Public] Constructor & Descructor=====
            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="aprop">組態設定依賴轉換之物件參數</param>
            public WebPropSettingDI(IOptions<WebPropSetting> aprop) : base(aprop) { }
            #endregion

            #region =====[Public] Method=====
            // 可自訂其他組態設定值(properties)運用的方法
            #endregion
        }

        /// <summary>
        /// 依賴類別注入服務之應用類別，取得指定之組態設定檔內容(Root)
        /// </summary>
        public class WebSettingRootDI : AppSettingsDI<WebSetting>
        {
            #region =====[Public] Constructor & Descructor=====
            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="aprop">組態設定依賴轉換之物件參數</param>
            public WebSettingRootDI(IOptions<WebSetting> aprop) : base(aprop) { }
            #endregion

            #region =====[Public] Method=====
            // 可自訂其他組態設定值(properties)運用的方法
            #endregion
        }

        /// <summary>
        /// 系統組態設定(appsettings.json)相關處理類別
        /// </summary>
        public class WebPropertySetting : PropertySetting<WebSetting, WebSettingRootDI>
        {
            #region =====[Private] Class=====
            /// <summary>
            /// 靜態初始化通用物件實體類別
            /// </summary>
            private static class LazyHolder
            {
                /// <summary>
                /// <c>WebPropertySetting</c> 通用物件實體
                /// </summary>
                /// <remarks>預設組態設定檔案"appsettings.json"位置為工作目錄，且系統組態設定檔之區段路徑為"WebPropSetting"</remarks>
                public static readonly WebPropertySetting INSTANCE = new WebPropertySetting("WebPropSetting");
            }
            #endregion

            #region =====[Public] Getter & Setter=====
            /// <summary>
            /// 系統組態設定(Configuration)根物件
            /// </summary>
            public WebSettingRootDI ConfigRoot { get; }
            #endregion

            #region =====[Public] Constructor & Destructor=====
            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="pSectionPath">系統組態設定檔之區段路徑，無指定則填入<c>null</c>或<c>string.Empty</c></param>
            public WebPropertySetting(string pSectionPath) : base(pSectionPath)
            {
                ConfigRoot = ServiceProvider.GetService<WebSettingRootDI>();
            }
            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="pSectionPath">系統組態設定檔之區段路徑，無指定則填入<c>null</c>或<c>string.Empty</c></param>
            /// <param name="pConfigPath">系統組態設定檔之檔案路徑名稱</param>
            public WebPropertySetting(string pSectionPath, string pConfigPath) : base(pSectionPath, pConfigPath)
            {
                ConfigRoot = ServiceProvider.GetService<WebSettingRootDI>();
            }
            #endregion

            #region =====[Public] Method=====
            /// <summary>
            /// 取得系統組態設定(Configuration)物件實體
            /// </summary>
            /// <returns><c>WebPropertySetting</c> 通用物件實體</returns>
            public static WebPropertySetting Instance()
            {
                return LazyHolder.INSTANCE;
            }
            #endregion
        }

        #endregion
    }
}
