using Microsoft.AspNetCore.Mvc;

using Mma.Cli.UI.Models;

using System.Diagnostics;

namespace Mma.Cli.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
               
    }
}
