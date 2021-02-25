using Lib.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BPMWebConsole.Models.DAO
{
    /// <summary>
    /// "DCS_CONF"資料表類別
    /// </summary>
    public class DCSConf : DBRecord
    {
        #region =====[Public] Class=====

        /// <summary>
        /// 資料表欄位物件
        /// </summary>
        public class Row
        {
            #region =====[Public] Getter & Setter=====
            /// <summary>
            /// 航空公司代碼，使用IATA格式
            /// </summary>
            public string DCS_ID { get; set; }
            /// <summary>
            /// 航空公司MQ伺服器IP位址
            /// </summary>
            public string IP_ADDR { get; set; }
            /// <summary>
            /// 航空公司MQ之佇列名稱
            /// </summary>
            public string QUEUE_NAME { get; set; }
            /// <summary>
            /// 與航空公司MQ伺服器連線處理之佇列管理程式名稱
            /// </summary>
            public string BROKER { get; set; }
            /// <summary>
            /// 此航空公司佇列是否為使用中
            /// </summary>
            /// <remarks>
            /// <para>Y: 使用</para>
            /// <para>N: 不使用</para>
            /// </remarks>
            public string IN_USE { get; set; }
            /// <summary>
            /// 註解
            /// </summary>
            public string REMARK { get; set; }
            /// <summary>
            /// 資料列修改人員
            /// </summary>
            public string UPD_USER { get; set; }
            /// <summary>
            /// 資料列變動時間，格式為yyyyMMddHHmmss
            /// </summary>
            public string UPD_TIME { get; set; }
            #endregion
        }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="pConn">資料庫連接物件</param>
        public DCSConf(IDbConnection pConn) : base(pConn)
        {
            DBOwner = "dbo";
            TableName = "DCS_CONF";
            FieldName = new string[] { "DCS_ID", "IP_ADDR", "QUEUE_NAME", "BROKER", "IN_USE", "REMARK",
                                        "UPD_USER", "UPD_TIME" };
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
            pSqlStr += "'" + oRow.DCS_ID + "'";
            pSqlStr += ", '" + oRow.IP_ADDR + "'";
            pSqlStr += ", '" + oRow.QUEUE_NAME + "'";
            pSqlStr += ", '" + oRow.BROKER + "'";
            pSqlStr += ", '" + oRow.IN_USE + "'";
            pSqlStr += ", '" + oRow.REMARK + "'";
            pSqlStr += ", '" + oRow.UPD_USER + "'";
            pSqlStr += ", '" + oRow.UPD_TIME + "'";
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
            pSqlStr += "[" + FieldName[0] + "] [varchar](10) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[1] + "] [varchar](20) NULL";
            pSqlStr += ", [" + FieldName[2] + "] [varchar](50)  NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[3] + "] [varchar](10) NULL";
            pSqlStr += ", [" + FieldName[4] + "] [varchar](1) NOT NULL";
            pSqlStr += ", [" + FieldName[5] + "] [varchar](50) NULL";
            pSqlStr += ", [" + FieldName[6] + "] [varchar](20) NULL";
            pSqlStr += ", [" + FieldName[7] + "] [varchar](20) NULL";
            pSqlStr += ")";

            return Execute(pSqlStr);
        }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 依據[DCS_ID], [QUEUE_NAME]篩選[dbo.DCS_CONF]資料表
        /// </summary>
        /// <param name="pDcsID">航空公司代碼</param>
        /// <param name="pQName">航空公司MQ之佇列名稱</param>
        /// <returns>
        /// <para> 0: 依條件搜尋的筆數</para>
        /// <para>-1: 例外錯誤</para>
        /// </returns>
        /// <remarks>使用"<c>RecordList</c>"取出所查詢的資料列</remarks>
        public int SelectByKey(string pDcsID, string pQName)
        {
            return SelectByCondition(string.Format(" WHERE {0} = '{1}' and {2} = '{3}'",
                                                    FieldName[0], pDcsID, FieldName[2], pQName));
        }

        /// <summary>
        /// 篩選全部[dbo.DCS_CONF]資料表
        /// </summary>
        /// <returns>
        /// <para> 0: 依條件搜尋的筆數</para>
        /// <para>-1: 例外錯誤</para>
        /// </returns>
        /// <remarks>使用"<c>RecordList</c>"取出所查詢的資料列</remarks>
        public int SelectFromTable()
        {
            return SelectBySQL(string.Format("SELECT * FROM {0}", FullTableName));
        }

        /// <summary>
        /// 更新一筆資料表記錄
        /// </summary>
        /// <param name="pObj">單筆資料列物件</param>
        /// <returns>"UPDATE SET" SQL指令執行狀態</returns>
        public int Update(object pObj)
        {
            Row oRow = pObj as Row;

            string sql = "UPDATE " + FullTableName;
            sql += " SET " + FieldName[1] + " = '" + oRow.IP_ADDR + "'";
            sql += ", " + FieldName[3] + " = '" + oRow.BROKER + "'";
            sql += ", " + FieldName[4] + " = '" + oRow.IN_USE + "'";
            sql += ", " + FieldName[5] + " = '" + oRow.REMARK + "'";
            sql += ", " + FieldName[6] + " = '" + oRow.UPD_USER + "'";
            sql += ", " + FieldName[7] + " = '" + oRow.UPD_TIME + "'";
            sql += " WHERE " + FieldName[0] + " = '" + oRow.DCS_ID + "'";
            sql += " and " + FieldName[2] + " = '" + oRow.QUEUE_NAME + "'";

            return Execute(sql);
        }

        #endregion
    }
}
