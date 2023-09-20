using Microsoft.AspNetCore.Mvc;

namespace SceneSherpa.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
