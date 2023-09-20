﻿using Microsoft.AspNetCore.Mvc;
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

        [Route("media/{id:int}/review/{reviewId:int}")]
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

        [HttpPost]
        [Route("/Media/{mediaId:int}/Reviews")]
        public IActionResult Create(int mediaId, Review review)
        {
            //Currently without saving state cannot find the correct user, fix this by using the cookies content to specify the correct user.
            review.User = _context.Users.Find(1);
            
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
    }
}
