using Microsoft.AspNetCore.Mvc;

namespace SceneSherpa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
