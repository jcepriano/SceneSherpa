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
        public IActionResult About()
        {
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View();
        }

        public IActionResult NotFound()
        {
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View();
        }
    }
}
