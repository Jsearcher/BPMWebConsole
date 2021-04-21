using BPMWebConsole.Models;
using BPMWebConsole.Models.Entities;
using BPMWebConsole.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BPMWebConsole.Controllers
{
    /// <summary>
    /// 首頁登入頁面控制類別
    /// </summary>
    [Route("Home")]
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
