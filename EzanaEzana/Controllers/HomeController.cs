using EzanaEzana.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EzanaEzana.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult React()
        {
            return View();
        }

        public IActionResult ReactDashboard()
        {
            return View();
        }

        public IActionResult ShadcnDemo()
        {
            return View();
        }

        public IActionResult Playground()
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