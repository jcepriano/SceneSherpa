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

            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/{user.Id}");
        }

        [Route("/users/login")]
        public IActionResult LoginForm()
        {
            ViewData["FailedLogin"] = TempData["FailedLogin"];
            return View();
        }

        [HttpPost]
        [Route("/users/login/attempt")]
        public IActionResult LoginAttempt(string username, string password)
        {
            HashAlgorithm sha = SHA256.Create();
            byte[] firstInputBytes = Encoding.ASCII.GetBytes(password);
            byte[] firstInputDigested = sha.ComputeHash(firstInputBytes);

            StringBuilder firstInputBuilder = new StringBuilder();
            foreach (byte b in firstInputDigested)
            {
                Console.Write(b + ", ");
                firstInputBuilder.Append(b.ToString("x2"));
            }
            string value = firstInputBuilder.ToString();

            if (_context.Users.Where(e => e.Username == username && e.Password == value).Any())
            {
                User user = _context.Users.Where(e => e.Username == e.Username).Single();
                return Redirect($"/users/{user.Id}");
            }
            else
            {
                TempData["FailedLogin"] = "Either your password or username is incorrect, please try again.";
                return Redirect("/users/login");
            }
        }
        
        [Route("/Users/{id:int}")]
        public IActionResult Show(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }
        
        [Route("users/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
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
