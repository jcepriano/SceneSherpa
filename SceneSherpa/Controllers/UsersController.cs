using Microsoft.AspNetCore.Mvc;
using SceneSherpa.DataAccess;

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

        [Route("/Users/{id}")]
        public IActionResult Show(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }
    }
}
