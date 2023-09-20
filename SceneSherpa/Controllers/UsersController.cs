using Microsoft.AspNetCore.Mvc;

namespace SceneSherpa.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
