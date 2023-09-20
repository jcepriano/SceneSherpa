using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;

namespace SceneSherpa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SceneSherpaContext _context;

        public HomeController(ILogger<HomeController> logger, SceneSherpaContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            return View();
        }
    }
}
