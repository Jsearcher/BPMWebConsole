using BPMWebConsole.Models.Service;
using BPMWebConsole.Models.Source;
using BPMWebConsole.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BPMWebConsole.Controllers
{
    /// <summary>
    /// 統計數量查詢頁面控制類別
    /// </summary>
    [Route("Statistics")]
    public class StatisticsController : Controller
    {
        /// <summary>
        /// 進入點頁面控制
        /// </summary>
        /// <returns>統計數量查詢頁面資料</returns>
        [Authorize]
        [Route("Index.html")]
        public IActionResult Index()
        {
            StatisticsViewModel modelData = new StatisticsViewModel();
            return View(modelData);
        }

        /// <summary>
        /// 依條件查詢BPM通訊統計數量
        /// </summary>
        /// <param name="modelData">查詢條件
        /// <para>DateStart: 起始日期</para>
        /// <para>DateEnd: 結束日期</para>
        /// <para>Airline: 所選擇之航空公司代碼(IATA)</para>
        /// </param>
        /// <returns>統計數量查詢頁面資料</returns>
        [Authorize]
        [HttpPost("QueryBPMResult"), ValidateAntiForgeryToken]
        public IActionResult QueryBPMResult(StatisticsViewModel modelData)
        {
            modelData.FormsModel.QueryStatus = -1;
            if ((modelData.FormsModel.DateEnd - modelData.FormsModel.DateStart).TotalDays <= 31)
            {
                string date_start = modelData.FormsModel.DateStart.ToString("yyyyMMdd");
                string date_end = modelData.FormsModel.DateEnd.ToString("yyyyMMdd");
                string airline = modelData.FormsModel.Airline;
                if (airline == "ALL")
                {
                    airline = null;
                }
                List<BPMProcCount> counts = new StatisticsService().GetBPMProcCount(date_start, date_end, airline);
                modelData.FormsModel.QueryStatus = counts == null ? 0 : 1;
                modelData.TableModel = counts;
            }
            
            return View("Index", modelData);
        }
    }
}
