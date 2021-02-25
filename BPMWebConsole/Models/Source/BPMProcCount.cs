using Newtonsoft.Json;

namespace BPMWebConsole.Models.Source
{
    /// <summary>
    /// BPM Server處理數量類別
    /// </summary>
    public class BPMProcCount
    {
        /// <summary>
        /// 統記日期，格式為"yyyy/MM/dd"
        /// </summary>
        [JsonProperty("ProcDate", Required = Required.Always, Order = 1)]
        public string ProcDate { get; set; }

        /// <summary>
        /// 航空公司屬性資料類別
        /// </summary>
        [JsonProperty("AirlineProperty", Required = Required.Always, Order = 2)]
        public AirlineProperty Airline { get; set; }

        /// <summary>
        /// BPM之MQ訊息處理數量
        /// </summary>
        [JsonProperty("Count", Required = Required.Always, Order = 3)]
        public int Count { get; set; }
    }
}
