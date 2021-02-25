using BPMWebConsole.Models.ConfigScript;
using BPMWebConsole.Models.DAO;
using BPMWebConsole.Models.Source;
using Lib.DB;
using Lib.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BPMWebConsole.Models.Service
{
    /// <summary>
    /// 提供查詢統計資料之資料服務
    /// </summary>
    public class StatisticsService
    {
        #region =====[Private] Web Configuration=====

        /// <summary>
        /// BPMDB供此Web Application使用之資料庫連線參數
        /// </summary>
        private static readonly string BPMDB_ConnStr = WebConfig.WebPropertySetting.Instance().ConfigRoot.properties.WebPropSetting.DBServer.BPMDB;

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 依查詢條件取得BPM Server處理訊息之數量統計
        /// </summary>
        /// <param name="dateStart">查詢起始日期</param>
        /// <param name="dateEnd">查詢結束日期</param>
        /// <param name="airline">航空公司代碼(IATA)</param>
        /// <returns>BPM Server處理訊息之數量統計列表</returns>
        public List<BPMProcCount> GetBPMProcCount(string dateStart, string dateEnd, string airline = null)
        {
            DataBase BPMDB = null;
            List<BPMProcCount> RowList = new List<BPMProcCount>();
            List<BPMLog.Row> RowList_BPM = new List<BPMLog.Row>();
            List<AirlineProperty> airlines = new ConfigurationService().GetAirlineCollectionList();

            try
            {
                BPMDB = DataBase.Instance(BPMDB_ConnStr);
                if (BPMDB.Conn == new DataBase(null).Conn)
                {
                    ErrorLog.Log("ERROR", "StatisticsService", "DB connection failed.");
                    return null; // 資料庫連線失敗
                }
                BPMLog TB_BPMLog = new BPMLog(BPMDB.Conn);
                if (TB_BPMLog.SelectByCostom(dateStart, dateEnd, airline) > 0)
                {
                    InfoLog.Log("BPMWebConsole", "StatisticsService", $"Query BPM Log [{dateStart} - {dateEnd}][Airline: {airline}] from BRS successfully.");
                    TB_BPMLog.RecordList.ForEach(obj => RowList_BPM.Add(obj as BPMLog.Row));
                }
                else
                {
                    InfoLog.Log("BPMWebConsole", "StatisticsService", $"There is non of BPM Log [{dateStart} - {dateEnd}][Airline: {airline}] from BRS to query.");
                }

                if (airlines != null && RowList_BPM.Count > 0)
                {
                    foreach (AirlineProperty prop in airlines)
                    {
                        List<BPMLog.Row> tempList_BPM = RowList_BPM.Where(row => row.BSM_FLIGHT.StartsWith(prop.IATA_Code)).ToList();
                        if (tempList_BPM.Count > 0)
                        {
                            var tempList_BPM_g = tempList_BPM.GroupBy(obj => new { obj.BSM_DATE }).Select(obj_g => (obj_g.Key.BSM_DATE, BPM_COUNT: obj_g.Count())).ToList();
                            tempList_BPM_g.ForEach(obj => RowList.Add(new BPMProcCount
                            {
                                ProcDate = DateTime.ParseExact(obj.BSM_DATE, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd"),
                                Airline = prop,
                                Count = obj.BPM_COUNT
                            }));
                        }
                    }
                    RowList = RowList.OrderBy(obj => obj.ProcDate).ThenBy(obj => obj.Airline.IATA_Code).ToList();

                    InfoLog.Log("BPMWebConsole", "StatisticsService", "Complete statistical BPM Log counting under specific conditions.");
                }
                else
                {
                    RowList = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("StatisticsService", ex);
                RowList_BPM = null;
            }
            finally
            {
                if (BPMDB != null)
                {
                    BPMDB.Close();
                }
            }

            return RowList;
        }

        #endregion
    }
}
