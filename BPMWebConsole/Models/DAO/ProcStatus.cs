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
    /// "PROC_STATUS"資料表類別
    /// </summary>
    public class ProcStatus : DBRecord
    {
        #region =====[Public] Class=====

        /// <summary>
        /// 資料表欄位物件
        /// </summary>
        public class Row
        {
            /// <summary>
            /// BPM Server執行程序名稱
            /// </summary>
            public string PROC_ID { get; set; }

            /// <summary>
            /// 各執行程序所屬伺服器名稱
            /// </summary>
            public string SERVER_ID { get; set; }

            /// <summary>
            /// 資料列變動時間，格式為yyyyMMddHHmmss
            /// </summary>
            public string DATE_TIME { get; set; }

            /// <summary>
            /// 各執行程序運作狀態
            /// </summary>
            /// <remarks>
            /// <para>not 1: 非執行中</para>
            /// <para>1: 執行中</para>
            /// </remarks>
            public int? STATUS { get; set; } = null;
        }

        #endregion


        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="pConn">資料庫連接物件</param>
        public ProcStatus(IDbConnection pConn) : base(pConn)
        {
            DBOwner = "dbo";
            TableName = "PROC_STATUS";
            FieldName = new string[] { "PROC_ID", "SERVER_ID", "DATE_TIME", "STATUS" };
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
            pSqlStr += "'" + oRow.PROC_ID + "'";
            pSqlStr += ", '" + oRow.SERVER_ID + "'";
            pSqlStr += ", '" + oRow.DATE_TIME + "'";
            pSqlStr += ", " + oRow.STATUS;
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
            pSqlStr += "[" + FieldName[0] + "] [varchar](20) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[1] + "] [varchar](20) NOT NULL PRIMARY KEY";
            pSqlStr += ", [" + FieldName[2] + "] [varchar](14) NULL";
            pSqlStr += ", [" + FieldName[3] + "] [int] NULL";
            pSqlStr += ")";

            return Execute(pSqlStr);
        }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 依據[PROC_ID], [SERVER_ID]篩選[dbo.PROC_STATUS]資料表
        /// </summary>
        /// <param name="pProcID">BPM Server執行程序名稱</param>
        /// <param name="pServerID">各執行程序所屬伺服器名稱</param>
        /// <returns>
        /// <para> 0: 依條件搜尋的筆數</para>
        /// <para>-1: 例外錯誤</para>
        /// </returns>
        /// <remarks>使用"<c>RecordList</c>"取出所查詢的資料列</remarks>
        public int SelectByKey(string pProcID, string pServerID)
        {
            return SelectByCondition(string.Format(" WHERE {0} = '{1}' and {2} = '{3}'",
                                                    FieldName[0], pProcID, FieldName[1], pServerID));
        }

        /// <summary>
        /// 篩選全部[dbo.PROC_STATUS]資料表
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
            sql += " SET " + FieldName[2] + " = '" + oRow.DATE_TIME + "'";
            sql += ", " + FieldName[3] + " = " + oRow.STATUS;
            sql += " WHERE " + FieldName[0] + " = '" + oRow.PROC_ID + "'";
            sql += " and " + FieldName[1] + " = '" + oRow.SERVER_ID + "'";

            return Execute(sql);
        }

        #endregion
    }
}
