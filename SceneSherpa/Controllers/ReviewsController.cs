using Microsoft.AspNetCore.Mvc;
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

        [Route("media/{id:int}/review")]
        public IActionResult Edit(int reviewId)
        {
            var user = _context.Users.Find(reviewId);
            return View(user);
        }

        [HttpPost]
        [Route("media/{id:int}")]
        public IActionResult Update(int id, Review review)
        {
            review.Id = id;
            _context.Reviews.Update(review);
            _context.SaveChanges();

            return Redirect("/media/{id:int}");
        }
    }
}
