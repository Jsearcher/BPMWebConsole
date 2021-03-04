using BPMWebConsole.Models.Service;
using BPMWebConsole.Models.Source;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPMWebConsole.Models.ViewModels
{
    /// <summary>
    /// 統計數量查詢頁面資料集合類別
    /// </summary>
    public class StatisticsViewModel
    {
        #region =====[Public] Class 統計數量查詢頁面資料集合類別=====

        /// <summary>
        /// 統計數量查詢頁面表單資料類別
        /// </summary>
        public class RegisterViewModel
        {
            /// <summary>
            /// 資料查詢起始日期
            /// </summary>
            [DataType(DataType.Date)]
            public DateTime DateStart { get; set; }

            /// <summary>
            /// 資料查詢結束日期
            /// </summary>
            [DataType(DataType.Date)]
            public DateTime DateEnd { get; set; }

            /// <summary>
            /// 下拉選單選擇之航空公司代碼(IATA)
            /// </summary>
            public string Airline { get; set; }

            /// <summary>
            /// 下拉選單航空公司集合
            /// </summary>
            public List<SelectListItem> Airlines { get; internal set; } = new List<SelectListItem>
            {
                new SelectListItem{ Value = "ALL", Text = "ALL" }
            };

            /// <summary>
            /// 資料查詢狀態
            /// </summary>
            /// <remarks>
            /// <para> null: 初始狀態</para>
            /// <para> 1: 查詢成功</para>
            /// <para> 0: 已查詢無結果；已查詢有錯誤</para>
            /// <para>-1: 查詢條件問題</para>
            /// </remarks>
            public int? QueryStatus { get; set; } = null;
        }

        #endregion

        #region =====[Public] Property=====

        /// <summary>
        /// Form表單註冊資料集合
        /// </summary>
        public RegisterViewModel FormsModel { get; set; }

        /// <summary>
        /// 統計查詢資料集合(適用顯示結果之圖或表)
        /// </summary>
        public List<BPMProcCount> TableModel { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <remarks>航空公司集合之選單用資料列表可能為null</remarks>
        public StatisticsViewModel()
        {
            List<AirlineProperty> airlines = new ConfigurationService().GetAirlineCollectionList();
            FormsModel = new RegisterViewModel()
            {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                Airline = "ALL"
            };
            if (airlines != null)
            {
                airlines.ForEach(airline => FormsModel.Airlines.Add(new SelectListItem
                {
                    Value = airline.IATA_Code,
                    Text = airline.Remark
                }));
            }
        }

        #endregion
    }
}
