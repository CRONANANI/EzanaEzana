using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ezana.Controllers
{
    [Authorize]
    public class ResearchController : Controller
    {
        private readonly ILogger<ResearchController> _logger;

        public ResearchController(ILogger<ResearchController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
