using Microsoft.AspNetCore.Mvc;

namespace SceneSherpa.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
