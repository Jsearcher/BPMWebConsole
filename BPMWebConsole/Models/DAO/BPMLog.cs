﻿using Lib.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace BPMWebConsole.Models.DAO
{
    /// <summary>
    /// "BPM_LOGXX"資料表類別
    /// </summary>
    /// <remarks>XX表示月份</remarks>
    public class BPMLog : DBRecord
    {
        #region =====[Public] Class=====

        /// <summary>
        /// 資料表欄位物件
        /// </summary>
        public class Row
        {
            #region =====[Public] Getter & Setter=====
            /// <summary>
            /// 行李所屬航班之作業日期，格式為yyyyMMdd
            /// </summary>
            public string BSM_DATE { get; set; }
            /// <summary>
            /// 行李條碼編號
            /// </summary>
            public string BAG_TAG { get; set; }
            /// <summary>
            /// 行李處理狀態
            /// </summary>
            /// <remarks>
            /// <para>0: 刪除</para>
            /// <para>1: 新增</para>
            /// <para>2: 更新</para>
            /// </remarks>
            public int? BSM_STATE { get; set; } = null;
            /// <summary>
            /// 行李裝載狀態
            /// </summary>
            /// <remarks>
            /// <para> 0: 新增</para>
            /// <para>10: 裝櫃</para>
            /// <para>11: 卸載</para>
            /// </remarks>
            public int? BAG_STATE { get; set; } = null;
            /// <summary>
            /// 行李裝載許可
            /// </summary>
            /// <remarks>
            /// <para>Y: 允許</para>
            /// <para>N: 不允許</para>
            /// </remarks>
            public string AUTH_LOAD { get; set; }
            /// <summary>
            /// 行李運送許可
            /// </summary>
            /// <remarks>
            /// <para>Y: 允許</para>
            /// <para>N: 不允許</para>
            /// </remarks>
            public string AUTH_TRANSPORT { get; set; }
            /// <summary>
            /// 行李BSM訊息字串
            /// </summary>
            public string BSM_SOURCE { get; set; }
            /// <summary>
            /// 行李BPM訊息字串(裝、卸載)
            /// </summary>
            public string BPM_SOURCE { get; set; }
            /// <summary>
            /// 資料列變動時間，格式為yyyyMMddHHmmss
            /// </summary>
            public string UPD_TIME { get; set; }
            /// <summary>
            /// 行李所屬航班編號
            /// </summary>
            /// <remarks>行李應裝載行搬</remarks>
            public string BSM_FLIGHT { get; set; }
            /// <summary>
            /// 行李裝載航班編號
            /// </summary>
            /// <remarks>行李實際裝載行搬</remarks>
            public string BAG_FLIGHT { get; set; }
            #endregion
        }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="pConn">資料庫連接物件</param>
        public BPMLog(IDbConnection pConn) : base(pConn)
        {
            DBOwner = "dbo";
            TableName = "BPM_LOG";
            FieldName = new string[] { "BSM_DATE", "BAG_TAG", 
                                        "BSM_STATE", "BAG_STATE", "AUTH_LOAD", "AUTH_TRANSPORT", 
                                        "BSM_SOURCE", "BPM_SOURCE", "UPD_TIME", "BSM_FLIGHT", "BAG_FLIGHT" };
        }

        #endregion

        #region =====[Protected] Base Method for Each Table=====

        /// <summary>
        /// 擷取一列資料表記錄
        /// </summary>
        /// <param name="pRs"><c>IDataReader</c>資料擷取物件</param>
        /// <returns>一列資料表記錄</returns>
        protected override object FetchRecord(IDataReader pRs)
        {
            Row oRow = new Row();

            try
            {
                List<PropertyInfo> props = new List<PropertyInfo>(oRow.GetType().GetProperties());
                for (int i = 0; i < pRs.FieldCount; i++)
                {
                    string readerName = pRs.GetName(i);
                    foreach (PropertyInfo prop in props)
                    {
                        if (readerName == prop.Name)
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                prop.SetValue(oRow, GetValueOrDefault<string>(pRs, i));
                            }
                            else
                            {
                                prop.SetValue(oRow, GetValueOrDefault<object>(pRs, i));
                            }
                            break;
                        }
                    }
                }

                return oRow;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增一列資料表記錄之對應值
        /// </summary>
        /// <param name="pSqlStr">"INSERT INTO" SQL指令</param>
        /// <param name="pObj">單筆資料列物件</param>
        /// <returns>指令執行狀態</returns>
        protected override int SetRecord(string pSqlStr, object pObj)
        {
            Row oRow = new Row();

            pSqlStr += " VALUES(";
            pSqlStr += "'" + oRow.BSM_DATE + "'";
            pSqlStr += ", '" + oRow.BAG_TAG + "'";
            pSqlStr += ", " + oRow.BSM_STATE;
            pSqlStr += ", " + oRow.BAG_STATE;
            pSqlStr += ", '" + oRow.AUTH_LOAD + "'";
            pSqlStr += ", '" + oRow.AUTH_TRANSPORT + "'";
            pSqlStr += ", '" + oRow.BSM_SOURCE + "'";
            pSqlStr += ", '" + oRow.BPM_SOURCE + "'";
            pSqlStr += ", '" + oRow.UPD_TIME + "'";
            pSqlStr += ", '" + oRow.BSM_FLIGHT + "'";
            pSqlStr += ", '" + oRow.BAG_FLIGHT + "'";
            pSqlStr += ")";

            return Execute(pSqlStr);
        }

        /// <summary>
        /// 設定資料表建立所需之欄位與資料型態
        /// </summary>
        /// <param name="pSqlStr">"CREATE TABLE" SQL指令</param>
        /// <returns>指令執行狀態</returns>
        protected override int SetField(string pSqlStr)
        {
            pSqlStr += " (";
            pSqlStr += "[" + FieldName[0] + "] [varchar](8) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[1] + "] [varchar](10) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[2] + "] [int] NULL";
            pSqlStr += ", [" + FieldName[3] + "] [int] NULL";
            pSqlStr += ", [" + FieldName[4] + "] [varchar](1) NULL";
            pSqlStr += ", [" + FieldName[5] + "] [varchar](1) NULL";
            pSqlStr += ", [" + FieldName[6] + "] [varchar](MAX) NULL";
            pSqlStr += ", [" + FieldName[7] + "] [varchar](MAX) NULL";
            pSqlStr += ", [" + FieldName[8] + "] [varchar](20) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[9] + "] [varchar](12) NOT NULL";
            pSqlStr += ", [" + FieldName[10] + "] [varchar](12) NOT NULL";
            pSqlStr += ")";

            return Execute(pSqlStr);
        }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 依據[BAG_TAG], [BSM_DATE]篩選[dbo.BPM_LOGXX]資料表
        /// </summary>
        /// <param name="pBagTag">行李條碼編號</param>
        /// <param name="pDate">行李所屬航班之作業日期</param>
        /// <returns>
        /// <para> 0: 依條件搜尋的筆數</para>
        /// <para>-1: 例外錯誤</para>
        /// </returns>
        /// <remarks>使用"<c>RecordList</c>"取出所查詢的資料列</remarks>
        public int SelectByKey(string pBagTag, string pDate)
        {
            pDate = string.IsNullOrEmpty(pDate) ?
                DateTime.Now.ToString("yyyyMMdd") :
                DateTime.ParseExact(pDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            TableName += pDate.Substring(4, 2);
            return SelectByCondition(string.Format(" WHERE {0} = '{1}' and {2} = '{3}'" +
                                                    " ORDER BY {4}, {2}",
                                                    FieldName[0], pDate, FieldName[1], pBagTag, FieldName[9]));
        }

        /// <summary>
        /// 依據[BSM_DATE], [BSM_FLIGHT]篩選[dbo.BPM_LOGXX]資料表，[BSM_FLIGHT]包含指定的航空公司代碼
        /// </summary>
        /// <param name="pDate1">行李所屬航班之作業日期(查詢起始)</param>
        /// <param name="pDate2">行李所屬航班之作業日期(查詢結束)</param>
        /// <param name="pAirline">航空公司代碼(IATA)</param>
        /// <returns>
        /// <para> 0: 依條件搜尋的筆數</para>
        /// <para>-1: 例外錯誤</para>
        /// </returns>
        /// <remarks>使用"<c>RecordList</c>"取出所查詢的資料列</remarks>
        public int SelectByCostom(string pDate1, string pDate2, string pAirline = null)
        {
            bool isOneDay = false;
            string oneDay = string.Empty;
            if (!string.IsNullOrEmpty(pDate1) && !string.IsNullOrEmpty(pDate2))
            {
                if (pDate1 == pDate2)
                {
                    isOneDay = true;
                    oneDay = pDate1;
                }
                else
                {
                    isOneDay = false;
                }
            }
            else if (!string.IsNullOrEmpty(pDate1))
            {
                isOneDay = true;
                oneDay = pDate1;
            }
            else if (!string.IsNullOrEmpty(pDate2))
            {
                isOneDay = true;
                oneDay = pDate2;
            }

            string temp1 = $@"
                SELECT {FieldName[0]}, {FieldName[1]}, {FieldName[9]}" + @"
                    FROM {0}" + @"
                    WHERE {1}" + $@"
                ORDER BY {FieldName[0]}, {FieldName[9]}, {FieldName[1]}";
            string temp2 = $@"
                SELECT {FieldName[0]}, {FieldName[1]}, {FieldName[9]}
                    FROM {FullTableName}{pDate1.Substring(4, 2)}" + @"
                    WHERE {0}" + $@"
                UNION ALL
                SELECT {FieldName[0]}, {FieldName[1]}, {FieldName[9]}
                    FROM {FullTableName}{pDate2.Substring(4, 2)}" + @"
                    WHERE {0}" + $@"
                ORDER BY {FieldName[0]}, {FieldName[9]}, {FieldName[1]}";

            string SqlStr;
            if (isOneDay)
            {
                string table = FullTableName + oneDay.Substring(4, 2);
                string cond = pAirline == null ? $"{FieldName[0]} = '{oneDay}'" : $"{FieldName[0]} = '{oneDay}' and {FieldName[9]} like '{pAirline}%'";
                SqlStr = string.Format(temp1, table, cond);
            }
            else
            {
                string cond = pAirline == null ? $"({FieldName[0]} between '{pDate1}' and '{pDate2}')" : $"({FieldName[0]} between '{pDate1}' and '{pDate2}') and {FieldName[9]} like '{pAirline}%'";
                if (pDate1.Substring(4, 2) == pDate2.Substring(4, 2))
                {
                    string table = FullTableName + pDate1.Substring(4, 2);
                    SqlStr = string.Format(temp1, table, cond);
                }
                else
                {
                    SqlStr = string.Format(temp2, cond);
                }
            }

            return SelectBySQL(SqlStr);
        }

        /// <summary>
        /// 更新一筆資料表記錄
        /// </summary>
        /// <param name="pObj">單筆資料列物件</param>
        /// <returns>"UPDATE SET" SQL指令執行狀態</returns>
        public int Update(Object pObj)
        {
            Row oRow = pObj as Row;

            string sql = "UPDATE " + FullTableName;
            sql += " SET " + FieldName[2] + " = " + oRow.BSM_STATE;
            sql += ", " + FieldName[3] + " = " + oRow.BAG_STATE;
            sql += ", " + FieldName[4] + " = '" + oRow.AUTH_LOAD + "'";
            sql += ", " + FieldName[5] + " = '" + oRow.AUTH_TRANSPORT + "'";
            sql += ", " + FieldName[6] + " = '" + oRow.BSM_SOURCE + "'";
            sql += ", " + FieldName[7] + " = '" + oRow.BPM_SOURCE + "'";
            sql += ", " + FieldName[8] + " = '" + oRow.UPD_TIME + "'";
            sql += ", " + FieldName[9] + " = '" + oRow.BSM_FLIGHT + "'";
            sql += ", " + FieldName[10] + " = '" + oRow.BAG_FLIGHT + "'";
            sql += " WHERE " + FieldName[0] + " = '" + oRow.BSM_DATE + "'";
            sql += " and " + FieldName[1] + " = '" + oRow.BAG_TAG + "'";

            return Execute(sql);
        }

        #endregion
    }
}
