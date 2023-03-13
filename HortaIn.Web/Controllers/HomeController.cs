using HortaIn.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HortaIn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Signin()
        {
            return View();
        }
        public IActionResult Recovery()
        {
            return View();
        }
         public IActionResult Change(string secret)
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