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

        [Route("media/{id:int}/reviews/{reviewId:int}")]
        public IActionResult Edit(int reviewId)
        {
            var user = _context.Users.Find(reviewId);
            return View(user);
        }

        [HttpPost]
        [Route("media/{id:int}")]
        public IActionResult Update(int id, Review review, int reviewId)
        {
            review.Id = reviewId;
            _context.Reviews.Update(review);
            _context.SaveChanges();

            return Redirect($"/media/{id}");
        }

        [HttpPost]
        [Route("/Media/{mediaId:int}/Reviews")]
        public IActionResult Create(int mediaId, Review review)
        {
            //Getting Currently Logged in User
            var currentUsername = Request.Cookies["CurrentUserIdUsername"].Split()[1];
            var currentUserId = _context.Users
                .Where(u => u.Username == currentUsername)
                .First()
                .Id;
            review.User = _context.Users.Find(currentUserId);
            
            var media = _context.Media
                .Where(m => m.Id == mediaId)
                .Include(m => m.Reviews)
                .First();
            review.CreatedAt = DateTime.Now.ToUniversalTime();
            media.Reviews.Add(review);
            _context.SaveChanges();
            return Redirect($"/Media/{mediaId}");
        }
        
        [Route("/Media/{mediaId:int}/Reviews/New")]
        public IActionResult New(int mediaId)
        {
            var media = _context.Media.Where(m => m.Id == mediaId).Include(m => m.Reviews).First();
            return View(media);
        }

        [HttpPost]
        [Route("media/{mediaId:int}/reviews/delete/{reviewId:int}")]
        public IActionResult Delete(int reviewId, int mediaId)
        {
            var review = _context.Reviews.Find(reviewId);
            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Redirect($"/media/{mediaId}");
        }
    }
}
