using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;

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

        [Route("/Media/{mediaId:int}/Reviews/New")]
        public IActionResult New(int mediaId)
        {
            var media = _context.Media.Where(m => m.Id == mediaId).Include(m => m.Reviews).First();
            return View(media);
        }

        [HttpPost]
        [Route("/Media/{mediaId:int}/Reviews")]
        public IActionResult Create(int mediaId, Review review)
        {
            var media = _context.Media
                .Where(m => m.Id == mediaId)
                .Include(m => m.Reviews)
                .First();
            review.CreatedAt = DateTime.Now.ToUniversalTime();
            media.Reviews.Add(review);
            _context.SaveChanges();
            return Redirect($"/Media/{mediaId}");
        }

    }
}
