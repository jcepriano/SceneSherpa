using Microsoft.AspNetCore.Mvc;
using SceneSherpa.DataAccess;

namespace SceneSherpa.Controllers
{
    public class MediaController : Controller
    {
        private readonly SceneSherpaContext _context;

        public MediaController(SceneSherpaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
