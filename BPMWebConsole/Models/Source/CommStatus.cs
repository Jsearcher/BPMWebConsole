using Newtonsoft.Json;

namespace BPMWebConsole.Models.Source
{
    /// <summary>
    /// 通訊連線運作狀態種類之列舉項目
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// BPM Server各程序之運作狀態集合
        /// </summary>
        BPMServer = 1,

        /// <summary>
        /// 各航空公司MQ之連線處理狀態集合
        /// </summary>
        AirlineMQ = 2,

        /// <summary>
        /// BRS2BPM程序之運作狀態集合
        /// </summary>
        BRS2BPM = 3
    }

    #region =====[Public] Class as Data Adaptor=====

    /// <summary>
    /// 通訊連線之運作狀態類別
    /// </summary>
    public class CommStatus
    {
        /// <summary>
        /// 通訊連線運作狀態種類
        /// </summary>
        [JsonProperty("Type", Required = Required.Default, Order = 1)]
        public StatusType Type { get; set; }

        /// <summary>
        /// 通訊連線對象或程序名稱(代號)
        /// </summary>
        [JsonProperty("Name_Code", Required = Required.Default, Order = 2)]
        public string Name_Code { get; set; }

        /// <summary>
        /// 通訊連線對象或程序名稱(顯示)
        /// </summary>
        [JsonProperty("Name_Desc", Required = Required.Default, Order = 3)]
        public string Name_Desc { get; set; }

        /// <summary>
        /// 通訊連線狀態
        /// </summary>
        [JsonProperty("Status", Required = Required.AllowNull, Order = 4)]
        public bool? Status { get; set; } = false;
    }

    #endregion
}
