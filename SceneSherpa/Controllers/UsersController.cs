using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;
using System.Security.Cryptography;
using System.Text;

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
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            return View();
        }


        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            //this properly hashes these properties and saves to Db. Method is located in *SceneSherpa.Models.User*
            user.Password = user.ReturnEncryptedString(user.Password);
            Response.Cookies.Append("CurrentUserId", $"{user.Id} , {user.Username}");
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];

            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/{user.Id}");
        }

        [Route("/users/login")]
        public IActionResult LoginForm()
        {
            //this may not work as intended, but previously it was completely incoherent.
            ViewData["FailedLogin"] = TempData["FailedLogin"];
            if (ViewData["CurrentUserId"]  == null)
            {
                ViewData["CurrentUserId"] = TempData["LoggedInUserId"];
            }
            else if (TempData["LoggedInUserId"] == null)
            {
                ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            }

            return View();
        }

        [HttpPost]
        [Route("/users/login/attempt")]
        public IActionResult LoginAttempt(string username, string password)
        {
            //Created an Admin User to allow the use of the 'ReturnEncyptedString' Method.. This can be changed, but this removed 15 lines.
            User Admin = new() { Name = "Admin", Email = "Admin@gmail.com", Age = 9999, Password = "Admin", Username = "Admin", Id = 9999 };

            if (_context.Users.Where(e => e.Username == username && e.Password == Admin.ReturnEncryptedString(password)).Any())
            {
                User user = _context.Users.Where(e => e.Username == e.Username).FirstOrDefault();

                //assign the cookie "CurrentUser" to a username. TempData[] CANNOT store an object.
                Response.Cookies.Append("CurrentUserId", $"{user.Id} , {user.Username}");

                return Redirect($"/users/{user.Id}");
            }
            else
            {
                TempData["FailedLogin"] = "Either your password or username is incorrect, please try again.";
                return Redirect("/users/login");
            }
        }

        [Route("/users/logout/{id:int}")] 
        public IActionResult Logout(int id)
        {
            if(id != null)
            {
                Response.Cookies.Delete("CurrentUserId");
            }

            return Redirect("/media");
        }
        
        [Route("/Users/{id:int}")]
        public IActionResult Show(int id)
        {
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            var user = _context.Users.Find(id);
            
            return View(user);
        }
        
        [Route("users/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];

            return View(user);
        }
        
        [HttpPost]
        [Route("users/{id:int}")]
        public IActionResult Update(int id, User user)
        {
            user.Id = id;
      
            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("show", new { id = user.Id });
        }
        
        [HttpPost]
        [Route("/Users/Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            //Grab user from context with all lists included
            var user = _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.AllWatched)
                .Include(u => u.CurrentWatch)
                .Include(u => u.ToWatch)
                .First();
            _context.Users.Remove(user);
            _context.SaveChanges();
            //Change the redirect to home page? 
            return RedirectToAction("index");
        }
    }
}
