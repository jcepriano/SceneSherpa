﻿using Microsoft.AspNetCore.Mvc;
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

            var media = _context.Media.Include(e => e.Reviews).ThenInclude(r => r.User);
            if (media == null) return NotFound();
            var currentUsernameId = Request.Cookies["CurrentUserIdUsername"];

            if(currentUsernameId != null)
            {
                List<string> userObj = new List<string>();
                userObj.AddRange(currentUsernameId.Split());

                var user = _context.Users.Find(Int32.Parse(userObj[0]));
                ViewData["User"] = user;
   //             user.FindUserbyUsername(user.Id);
                ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
                ViewBag.MediaList = _context.Media.ToList();
            }
            ViewBag.MediaList = _context.Media.ToList();
            return View(media);
        }

        [Route("media/{id:int}")]
        public IActionResult Show(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var media = _context.Media
                .Where(m => m.Id == id)
                .Include(media => media.Reviews)
                .ThenInclude(review => review.User)
                .FirstOrDefault();
            if (media == null) return NotFound();
            ViewBag.MediaList = _context.Media.ToList();

            //JK: find current user and pass in their review as a seperate review object, If no one is logged in, pass no user reviews in
            if (Request.Cookies["CurrentUserIdUsername"] != null)
            {
                var currentUsername = Request.Cookies["CurrentUserIdUsername"].Split()[1];
                var currentUser = _context.Users
                    .Where(u => u.Username == currentUsername)
                    .Include(u => u.AllWatched)
                    .Include(u => u.ToWatch)
                    .Include(u => u.CurrentWatch)
                    .First();
                ViewData["CurrentUserObject"] = currentUser;
                if (media.Reviews.Any(r => r.User == currentUser))
                {
                    ViewData["CurrentUserReview"] = media.Reviews.Where(r => r.User == currentUser).First();
                    
                }
            }

            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View(media);
        }
    }
}
