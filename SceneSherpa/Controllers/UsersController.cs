using Microsoft.AspNetCore.Mvc;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;

namespace SceneSherpa.Controllers
{
    public class UsersController : Controller
    {
        private readonly SceneSherpaContext _context;

        public UsersController(SceneSherpaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/");
        }
    }
}
