using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var media = _context.Media;
            return View(media);
        }

        [Route("media/{id:int}")]
        public IActionResult Show(int id)
        {
            var media = _context.Media
                .Where(m => m.Id == id)
                .Include(media => media.Reviews)
                .FirstOrDefault();

            return View(media);
        }
    }
}
