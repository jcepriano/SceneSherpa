using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //this properly hashes these properties and saves to Db. Method is located in *SceneSherpa.Models.User*
            user.Name = user.ReturnEncryptedString(user.Name);
            user.Email = user.ReturnEncryptedString(user.Email);
            user.Password = user.ReturnEncryptedString(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/{user.Id}");
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
