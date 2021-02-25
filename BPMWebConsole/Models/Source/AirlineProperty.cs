using Newtonsoft.Json;

namespace BPMWebConsole.Models.Source
{
    #region =====[Public] Class as Data Adaptor=====

    /// <summary>
    /// 與BPM Server作MQ通訊之航空公司屬性資料類別
    /// </summary>
    /// <remarks>包含航空公司代碼(IATA)、航空公司註記名稱(Remark)</remarks>
    public class AirlineProperty
    {
        /// <summary>
        /// 航空公司代碼(IATA)
        /// </summary>
        [JsonProperty("IATA_Code", Required = Required.Always, Order = 1)]
        public string IATA_Code { get; set; }

        /// <summary>
        /// 航空公司代碼(ICAO)
        /// </summary>
        [JsonProperty("ICAO_Code", Required = Required.Default, Order = 2)]
        public string ICAO_Code { get; set; }

        /// <summary>
        /// 航空公司註記名稱
        /// </summary>
        [JsonProperty("Remark", Required = Required.AllowNull, Order = 3)]
        public string Remark { get; set; }
    }

    #endregion
}
