using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using automanager.Models;
using automanager.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace automanager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CarManagerDatabase _db;

        public HomeController(ILogger<HomeController> logger, CarManagerDatabase db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_db.Vehicles.Where(v => v.Visible).OrderBy(v => v.DisplayOrder));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetInt32("_access", 0);
            await HttpContext.Session.CommitAsync();
            return RedirectToAction(nameof(Login));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
