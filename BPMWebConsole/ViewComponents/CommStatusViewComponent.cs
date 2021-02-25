using BPMWebConsole.Models.Service;
using BPMWebConsole.Models.Source;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BPMWebConsole.ViewComponents
{
    /// <summary>
    /// ViewComponent for "Dashboard/CommStatus.cshtml"
    /// </summary>
    [ViewComponent(Name = "Dashboard")]
    public class CommStatusViewComponent : ViewComponent
    {
        /// <summary>
        /// 注入 <c>CommStatus</c> 通訊連線狀態之資料取得服務
        /// </summary>
        /// <param name="type">通訊連線運作狀態種類</param>
        /// <returns>通訊連線狀態參數清單</returns>
        public async Task<IViewComponentResult> InvokeAsync(StatusType type)
        {
            var statuses = await Task.FromResult(new CommStatusService().GetCommStatus(type));
            switch (type)
            {
                case StatusType.BPMServer:
                    ViewData["status_table_name"] = "BPM程序運作狀態";
                    ViewData["status_column_name"] = "BPM程序";
                    break;
                case StatusType.AirlineMQ:
                    ViewData["status_table_name"] = "航空公司BPM接收狀態";
                    ViewData["status_column_name"] = "航空公司";
                    break;
                case StatusType.BRS2BPM:
                    ViewData["status_table_name"] = "BRS傳送BPM連線狀態";
                    ViewData["status_column_name"] = "主機";
                    break;
                default:
                    ViewData["status_table_name"] = "運作狀態表名稱";
                    ViewData["status_column_name"] = "運作狀態表欄位名稱";
                    break;
            }
            return View("CommStatus", statuses);
        }
    }
}
