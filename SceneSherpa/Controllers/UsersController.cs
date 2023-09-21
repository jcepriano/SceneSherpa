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
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View(_context.Users.ToList());
        }

        public IActionResult New()
        {
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            //this properly hashes these properties and saves to Db. Method is located in *SceneSherpa.Models.User*
            user.Password = user.ReturnEncryptedString(user.Password);

            bool usernames = _context.Users.Any(u => u.Username == user.Username);

            if (usernames)
            {
                ModelState.AddModelError("Username", "Username already exists");
                TempData["ErrorMessage"] = "Username already exists";
                return View("New", user);
            }

            Response.Cookies.Append("CurrentUserIdUsername", $"{user.Id} {user.Username}");

            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];

            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/{user.Id}");
        }

        [Route("/users/login")]
        public IActionResult LoginForm()
        {
            ViewData["FailedLogin"] = TempData["FailedLogin"];

            //if (ViewData["CurrentUserIdUsername"] == null)
            //{
            //    ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrenCurrentUserIdUsernametUser"];
            //}
            //else if (TempData["CurrentUserIdUsername"] == null)
            //{
            //    ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrenCurrentUserIdUsernametUser"];
            //}

            return View();
        }

        [HttpPost]
        [Route("/users/login/attempt")]
        public IActionResult LoginAttempt(string username, string password)
        {
            //User Admin = new() { Username = "Admin", Name = "Admin", Age = 0000, Email = "Admin@gmail.com", Id = 0000, Password = "Admin" };

            var LoginAttemptUser = _context.Users.Where(e => e.Username == username).FirstOrDefault();

            if(LoginAttemptUser.Password == LoginAttemptUser.ReturnEncryptedString(password))
            {
                Response.Cookies.Append("CurrentUserIdUsername", $"{LoginAttemptUser.Id} {LoginAttemptUser.Username}");
                return Redirect($"/users/{LoginAttemptUser.Id}");
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
                Response.Cookies.Delete("CurrentUserIdUsername");
            }

            return Redirect("/media");
        }
        
        [Route("/Users/{id:int}")]
        public IActionResult Show(int id)
        {
            var user = _context.Users.Find(id);
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View(user);
        }
        
        [Route("users/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];

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
