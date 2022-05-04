using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingSystem.Data;
using HotelBookingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingSystem.Controllers
{
    [Authorize] // 启用身份控制
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // private readonly HotelBookingSystemContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // public HomeController(ILogger<HomeController> logger, HotelBookingSystemContext context)
        // {
        //     _logger = logger;
        //     _context = context;
        // }
        //
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // [AllowAnonymous] // 创建用户前,给Register模型feed一些数据
        // public IActionResult HotelUserRegister()
        // {
        //     ViewBag.MembershipTypes = _context.MembershipType.ToList();
        //     return View("/Areas/Identity/Pages/Account/Register.cshtml"); // return to the generated register form
        // }
    }
}
