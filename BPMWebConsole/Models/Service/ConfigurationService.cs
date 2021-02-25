using BPMWebConsole.Models.ConfigScript;
using BPMWebConsole.Models.DAO;
using BPMWebConsole.Models.Source;
using Lib.DB;
using Lib.Log;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BPMWebConsole.Models.Service
{
    /// <summary>
    /// 提供資料表各項基本組態設定之資料服務
    /// </summary>
    public class ConfigurationService
    {
        #region =====[Private] Web Configuration=====

        /// <summary>
        /// BPMDB供此Web Application使用之資料庫連線參數
        /// </summary>
        private static readonly string BPMDB_ConnStr = WebConfig.WebPropertySetting.Instance().ConfigRoot.properties.WebPropSetting.DBServer.BPMDB;

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 取得與BPM作MQ通訊之航空公司集合字典
        /// </summary>
        /// <returns>航空公司集合字典</returns>
        /// <remarks>不重複(排除QUEUE_NAME開頭TAISNR)</remarks>
        public Dictionary<string, string> GetAirlineCollectionDict()
        {
            Dictionary<string, string> CodeDict = new Dictionary<string, string>();
            List<DCSConf.Row> RowList = GetDCSConfList();

            if (RowList != null)
            {
                RowList.Where(row => !row.QUEUE_NAME.StartsWith("TAISNR")).ToList()
                    .ForEach(row => CodeDict.Add(row.DCS_ID, row.REMARK));
            }
            else
            {
                CodeDict = null;
            }

            return CodeDict;
        }

        /// <summary>
        /// 取得與BPM作MQ通訊之航空公司集合列表
        /// </summary>
        /// <returns>航空公司集合列表</returns>
        /// <remarks>不重複(排除QUEUE_NAME開頭TAISNR)</remarks>
        public List<AirlineProperty> GetAirlineCollectionList()
        {
            List<AirlineProperty> CodeList = new List<AirlineProperty>();
            List<DCSConf.Row> RowList = GetDCSConfList();

            if (RowList != null)
            {
                RowList.Where(row => !row.QUEUE_NAME.StartsWith("TAISNR")).ToList()
                    .ForEach(row => CodeList.Add(new AirlineProperty()
                    {
                        IATA_Code = row.DCS_ID,
                        ICAO_Code = string.Empty,
                        Remark = row.REMARK
                    }));
            }
            else
            {
                CodeList = null;
            }

            return CodeList;
        }

        #endregion

        #region =====[Private] Function=====

        /// <summary>
        /// 取得航空公司組態設定列表
        /// </summary>
        /// <returns>航空公司組態設定列表</returns>
        private List<DCSConf.Row> GetDCSConfList()
        {
            DataBase BPMDB = null;
            List<DCSConf.Row> RowList = new List<DCSConf.Row>();

            try
            {
                BPMDB = DataBase.Instance(BPMDB_ConnStr);
                if (BPMDB.Conn == new DataBase(null).Conn)
                {
                    ErrorLog.Log("ERROR", "ConfigurationService", "DB connection failed.");
                    return null; // 資料庫連線失敗
                }
                DCSConf TB_DCSConf = new DCSConf(BPMDB.Conn);
                if (TB_DCSConf.SelectFromTable() > 0)
                {
                    InfoLog.Log("BPMWebConsole", "ConfigurationService", "Query DCS configuration successfully.");
                    TB_DCSConf.RecordList.ForEach(obj => RowList.Add(obj as DCSConf.Row));
                }
                else
                {
                    InfoLog.Log("BPMWebConsole", "ConfigurationService", "There is non of DCS configuration to query.");
                    RowList = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("ConfigurationService", ex);
                RowList = null;
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
