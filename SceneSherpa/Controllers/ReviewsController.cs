using Microsoft.AspNetCore.Mvc;
using SceneSherpa.DataAccess;

namespace SceneSherpa.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly SceneSherpaContext _context;

        public ReviewsController(SceneSherpaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
