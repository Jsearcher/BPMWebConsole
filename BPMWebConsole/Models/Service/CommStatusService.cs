using BPMWebConsole.Models.ConfigScript;
using BPMWebConsole.Models.DAO;
using BPMWebConsole.Models.Source;
using Lib.DB;
using Lib.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BPMWebConsole.Models.Service
{
    /// <summary>
    /// <c>CommStatus</c> 通訊連線狀態之資料服務
    /// </summary>
    public class CommStatusService
    {
        #region =====[Private] Web Configuration=====

        /// <summary>
        /// BPMDB供此Web Application使用之資料庫連線參數
        /// </summary>
        private static readonly string BPMDB_ConnStr = WebConfig.WebPropertySetting.Instance().DBServer.BPMDB;

        /// <summary>
        /// 此Web Application執行模式
        /// </summary>
        /// <remarks>
        /// <para>0: 表示為測試模式</para>
        /// <para>1: 表示為正式運行模式</para>
        /// </remarks>
        private static readonly int Mode = WebConfig.WebPropertySetting.Instance().Basic.Mode;
        /// <summary>
        /// 此Web Application執行模式為測試模式時，所使用的測試日期，格式為"yyyy-MM-dd"
        /// </summary>
        private static readonly string TestDate = WebConfig.WebPropertySetting.Instance().Basic.TestDate;

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 取得通訊連線狀態集合
        /// </summary>
        /// <param name="type">通訊連線運作狀態種類</param>
        /// <returns><c>CommStatus</c> 通訊連線狀態列表</returns>
        public List<CommStatus> GetCommStatus(StatusType type)
        {
            var data = GetFakeCommStatusData();
            return type switch
            {
                StatusType.BPMServer => GetBPMServerProcStatus(),
                StatusType.AirlineMQ => GetAirlineMQStatus(),
                StatusType.BRS2BPM => data.Where(x => x.Type == type).ToList(),
                _ => data.Where(x => x.Type == type).ToList(),
            };
        }

        #endregion

        #region =====[Private] Function=====

        /// <summary>
        /// 取得BPM Server各程序之運作狀態
        /// </summary>
        /// <returns>BPM Server各程序之通訊連線狀態列表</returns>
        private List<CommStatus> GetBPMServerProcStatus()
        {
            DataBase BPMDB = null;
            List<CommStatus> RowList = new List<CommStatus>();

            try
            {
                BPMDB = DataBase.Instance(BPMDB_ConnStr);
                if (BPMDB.Conn == new DataBase(null).Conn)
                {
                    ErrorLog.Log("ERROR", "CommStatusService", "DB connection failed.");
                    return null; // 資料庫連線失敗
                }
                ProcStatus TB_ProcStatus = new ProcStatus(BPMDB.Conn);
                if (TB_ProcStatus.SelectFromTable() > 0)
                {
                    InfoLog.Log("BPMWebConsole", "CommStatusService", "Query process status successfully.");
                    TB_ProcStatus.RecordList.ForEach(obj => RowList.Add(new CommStatus()
                    {
                        Type = StatusType.BPMServer,
                        Name_Code = null,
                        Name_Desc = (obj as ProcStatus.Row).PROC_ID,
                        Status = (obj as ProcStatus.Row).STATUS == 1
                    }));
                }
                else
                {
                    InfoLog.Log("BPMWebConsole", "CommStatusService", "There is non of process status record to query.");
                    RowList = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CommStatusService", ex);
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

        /// <summary>
        /// 取得Airline各MQ通訊之訊息接收狀態
        /// </summary>
        /// <returns>Airline各MQ通訊之訊息接收狀態列表</returns>
        private List<CommStatus> GetAirlineMQStatus()
        {
            string targetDate = Mode == 0 ?
                DateTime.ParseExact(TestDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd") :
                DateTime.Now.ToString("yyyyMMdd");
            DataBase BPMDB = null;
            List<CommStatus> RowList = new List<CommStatus>();
            List<BPMLast.Row> RowList_BPM = new List<BPMLast.Row>();
            Dictionary<string, string> AirlineDict = new ConfigurationService().GetAirlineCollectionDict();
            string RemMQ = WebConfig.WebPropertySetting.Instance().TempRecord.AirlineMQ;

            try
            {
                BPMDB = DataBase.Instance(BPMDB_ConnStr);
                if (BPMDB.Conn == new DataBase(null).Conn)
                {
                    ErrorLog.Log("ERROR", "CommStatusService", "DB connection failed.");
                    return null; // 資料庫連線失敗
                }
                BPMLast TB_BPMLast = new BPMLast(BPMDB.Conn);
                if (TB_BPMLast.SelectByDate(targetDate) > 0)
                {
                    InfoLog.Log("BPMWebConsole", "CommStatusService", "Query last BPM from BRS successfully.");
                    TB_BPMLast.RecordList.ForEach(obj => RowList_BPM.Add((obj as BPMLast.Row)));
                }
                else
                {
                    InfoLog.Log("BPMWebConsole", "CommStatusService", "There is non of last BPM from BRS to query.");
                }

                if (AirlineDict != null)
                {
                    // Parse remaining AirlineMQ
                    Dictionary<string, int> RemMQDict = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(RemMQ))
                    {
                        string[] Rems = RemMQ.Split(";");
                        foreach (string rMQ in Rems)
                        {
                            string[] partMQ = rMQ.Split(":");
                            RemMQDict.Add(partMQ[0], int.Parse(partMQ[1]));
                        }
                    }

                    // Compare data in BPMLast (if any) with Remaining AirlineMQ records
                    RemMQ = string.Empty;
                    foreach (string code in AirlineDict.Keys)
                    {
                        if (RowList_BPM.Count > 0)
                        {
                            if (RemMQDict.TryGetValue(code, out int rMQ_Last))
                            {
                                RemMQDict[code] = 0;
                            }
                            else
                            {
                                RemMQDict.Add(code, 0);
                            }
                            int rMQ_New = RowList_BPM.Where(row => row.BSM_FLIGHT.StartsWith(code)).Count();
                            RowList.Add(new CommStatus
                            {
                                Type = StatusType.AirlineMQ,
                                Name_Code = code,
                                Name_Desc = AirlineDict[code],
                                Status = rMQ_New < rMQ_Last || rMQ_New == 0
                            });
                            RemMQDict[code] = rMQ_New;
                        }
                        else
                        {
                            if (RemMQDict.TryGetValue(code, out int rMQ_Last))
                            {
                                RemMQDict[code] = 0;
                            }
                            else
                            {
                                RemMQDict.Add(code, 0);
                            }
                            RowList.Add(new CommStatus
                            {
                                Type = StatusType.AirlineMQ,
                                Name_Code = code,
                                Name_Desc = AirlineDict[code],
                                Status = true
                            });
                        }
                    }

                    foreach (KeyValuePair<string, int> rMQ in RemMQDict)
                    {
                        if (string.IsNullOrEmpty(RemMQ))
                        {
                            RemMQ += string.Format("{0}:{1}", rMQ.Key, rMQ.Value);
                        }
                        else
                        {
                            RemMQ += string.Format(";{0}:{1}", rMQ.Key, rMQ.Value);
                        }
                    }
                    WebConfig.WebPropertySetting.Instance().TempRecord.AirlineMQ = RemMQ;
                    InfoLog.Log("BPMWebConsole", "CommStatusService", "Update remaining AirlineMQ in web configuration successfully.");
                }
                else
                {
                    RowList = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("CommStatusService", ex);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<CommStatus> GetFakeCommStatusData()
        {
            List<CommStatus> statuses = new List<CommStatus>()
            {
                new CommStatus()
                {
                    Type = StatusType.BPMServer,
                    Name_Code = null,
                    Name_Desc = "TASKBATCH",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.BPMServer,
                    Name_Code = null,
                    Name_Desc = "TASKDCS",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.AirlineMQ,
                    Name_Code = "CI",
                    Name_Desc = "華航",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.AirlineMQ,
                    Name_Code = "BR",
                    Name_Desc = "長榮",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.AirlineMQ,
                    Name_Code = "JX",
                    Name_Desc = "星宇",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.AirlineMQ,
                    Name_Code = "CX",
                    Name_Desc = "國泰",
                    Status = true
                },
                new CommStatus()
                {
                    Type = StatusType.AirlineMQ,
                    Name_Code = "AK",
                    Name_Desc = "亞洲航空",
                    Status = false
                },
                new CommStatus()
                {
                    Type = StatusType.BRS2BPM,
                    Name_Code = null,
                    Name_Desc = "BRS主機",
                    Status = true
                }
            };

            return statuses;
        }

        #endregion
    }
}
