using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;
using Serilog;

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
        public IActionResult Edit(int? reviewId)
        {
            if (reviewId == null)
            {
                return BadRequest();
            }
            ViewBag.MediaList = _context.Media.ToList();
            var review = _context.Reviews.Where(r => r.Id == reviewId).Include(r => r.Media).First();
            if (review == null)
            {
                return NotFound();
            }
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];

            return View(review);
        }

        [HttpPost]
        [Route("media/{mediaId:int}/reviews/{reviewId:int}")]
        public IActionResult Update(int mediaId, Review review, int? reviewId)
        {
            if (reviewId == null)
            {
                return BadRequest();
            }
            ViewBag.MediaList = _context.Media.ToList();
            review.Id = (int)reviewId;
            review.UpdatedAt = DateTime.Now.ToUniversalTime();
            review.Content = Markdown.Parse(review.Content);
            try
            {
            _context.Reviews.Update(review);
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Fatal("Error when updating review in the database" + ex.Message);
            }


            return Redirect($"/media/{mediaId}");
        }

        [HttpPost]
        [Route("/Media/{mediaId:int}/Reviews")]
        public IActionResult Create(int? mediaId, Review review)
        {
            if (mediaId == null)
            {
                return BadRequest();
            }
            //Getting Currently Logged in User
            var currentUsername = Request.Cookies["CurrentUserIdUsername"].Split()[1];
            var currentUserId = _context.Users
                .Where(u => u.Username == currentUsername)
                .First()
                .Id;
            review.User = _context.Users.Find(currentUserId);

            var media = FindMediaWithReviewsById(mediaId);
           
            if (media == null)
            {
                return NotFound();
            }
            review.CreatedAt = DateTime.Now.ToUniversalTime();
            review.Content = Markdown.Parse(review.Content);
            try
            {
            media.Reviews.Add(review);
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Fatal("Error when adding review to the database");
            }

            return Redirect($"/Media/{mediaId}");
        }

        [Route("/Media/{mediaId:int}/Reviews/New")]
        public IActionResult New(int? mediaId)
        {
            if (mediaId == null)
            {
                return BadRequest();
            }
            ViewBag.MediaList = _context.Media.ToList();
            var media = FindMediaWithReviewsById(mediaId);
            if (media == null)
            {
                return NotFound();
            }
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View(media);
        }

        [HttpPost]
        [Route("media/{mediaId:int}/reviews/delete/{reviewId:int}")]
        public IActionResult Delete(int? reviewId, int mediaId)
        {
            if (reviewId == null)
            {
                return BadRequest();
            }
            var review = _context.Reviews.Find(reviewId);
            if (review == null)
            {
                return NotFound();
            }
            try
            {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Fatal("Error when removing review from database" + ex.Message);
            }


            return Redirect($"/media/{mediaId}");
        }

        public Media FindMediaWithReviewsById(int? mediaId)
        {
            return _context.Media
                .Where(m => m.Id == mediaId)
                .Include(m => m.Reviews)
                .First();

        }
    }
}
