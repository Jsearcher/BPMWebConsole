using BPMWebConsole.Models.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPMWebConsole.Controllers
{
    /// <summary>
    /// 通訊連線狀態儀表板頁面控制類別
    /// </summary>
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        /// <summary>
        /// 進入點頁面控制
        /// </summary>
        /// <returns>通訊連線狀態參數清單</returns>
        [Authorize]
        [Route("Index.html")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 依通訊連線運作狀態種類取得目前該狀態之顯示元件
        /// </summary>
        /// <param name="id">通訊連線運作狀態種類
        /// <para>1: <c>StatusType.BPMServer</c></para>
        /// <para>2: <c>StatusType.AirlineMQ</c></para>
        /// <para>3: <c>StatusType.BRS2BPM</c></para>
        /// </param>
        /// <returns>狀態之顯示元件</returns>
        [Authorize]
        [HttpGet("GetCommStatusVC/{id:int}")]
        public IActionResult GetCommStatusVC(int id)
        {
            ViewComponentResult VCResult = id switch
            {
                1 => ViewComponent("Dashboard", new { type = StatusType.BPMServer }),
                2 => ViewComponent("Dashboard", new { type = StatusType.AirlineMQ }),
                3 => ViewComponent("Dashboard", new { type = StatusType.BRS2BPM }),
                _ => ViewComponent("Dashboard", new { type = StatusType.BPMServer }),
            };
            return VCResult;
        }
    }
}
