using FindKočka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FindKočka.Data;
using FindKočka.Services;

namespace FindKočka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FindKočkaContext _context;

        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, FindKočkaContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.Name != null)
            {
                ViewBag.userId = _userService.GetUserId(this.User);
            }

            else
            {
                ViewBag.userId = null;
            }

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
    }
}
