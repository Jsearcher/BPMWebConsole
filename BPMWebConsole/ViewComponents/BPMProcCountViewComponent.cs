using BPMWebConsole.Models.Source;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BPMWebConsole.ViewComponents
{
    /// <summary>
    /// ViewComponent for "Statistics/BPMProcCount.cshtml"
    /// </summary>
    [ViewComponent(Name = "Statistics")]
    public class BPMProcCountViewComponent : ViewComponent
    {
        /// <summary>
        /// 注入 <c>BPMProcCount</c> BPM Server處理數量之長條圖取得服務
        /// </summary>
        /// <param name="counts">BPM Server處理數量統計列表</param>
        /// <returns>BPM Server處理數量之長條圖(View Component)</returns>
        public IViewComponentResult Invoke(List<BPMProcCount> counts)
        {
            return View("BPMProcCount", counts);
        }
    }
}
