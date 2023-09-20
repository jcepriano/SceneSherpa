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

            // JK: pass currently logged in user's review to show page
            try //JK: try to find a users review for this movie
            {
                var currentUser = _context.Users.Where(u => u.Username == Request.Cookies["CurrentUser"]).First();
                ViewData["CurrentUserReview"] = media.Reviews.Where(r => r.User == currentUser);
            }
            catch (Exception e) //JK: if doesnt exist the viewdata is set to null
            {
                ViewData["CurrentUserReview"] = null;
            }
            

            return View(media);
        }
    }
}
