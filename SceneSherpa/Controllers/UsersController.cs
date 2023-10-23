using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;
using Serilog;
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
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            return View(_context.Users.ToList());
        }

        public IActionResult New()
        {
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            ViewBag.MediaList = _context.Media.ToList();
            //this properly hashes these properties and saves to Db. Method is located in *SceneSherpa.Models.User*
            user.Password = user.ReturnEncryptedString(user.Password);

            bool usernames = _context.Users.Any(u => u.Username == user.Username);

            if (usernames)
            {
                ModelState.AddModelError("Username", "Username already exists");
                TempData["ErrorMessage"] = "Username already exists";
                return Redirect("/users/new");
            }
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Fatal("Error when adding new user to database");
            }


            Response.Cookies.Append("CurrentUserIdUsername", $"{user.Id} {user.Username}");

            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];

            return Redirect($"/users/{user.Id}");
        }

        [Route("/users/login")]
        public IActionResult LoginForm()
        {
            ViewBag.MediaList = _context.Media.ToList();
            ViewData["FailedLogin"] = TempData["FailedLogin"];
            return View();
        }

        [HttpPost]
        [Route("/users/login/attempt")]
        public IActionResult LoginAttempt(string username, string password)
        {
            //User Admin = new() { Username = "Admin", Name = "Admin", Age = 0000, Email = "Admin@gmail.com", Id = 0000, Password = "Admin" };
            if (username != null && password != null)
            {
                var LoginAttemptUser = _context.Users.Where(e => e.Username == username).FirstOrDefault();

                if (LoginAttemptUser.Password != null && LoginAttemptUser.Password == LoginAttemptUser.ReturnEncryptedString(password))
                {
                    Response.Cookies.Append("CurrentUserIdUsername", $"{LoginAttemptUser.Id} {LoginAttemptUser.Username}");
                    return Redirect($"/users/{LoginAttemptUser.Id}");
                }
                else
                {
                    TempData["FailedLogin"] = "Either your password or username is incorrect, please try again.";
                }
            }
            return Redirect("/users/login");
        }

        [Route("/users/{id:int}/Logout")]
        public IActionResult Logout(int id)
        {
            ViewBag.MediaList = _context.Media.ToList();
            if (id != null)
            {
                Response.Cookies.Delete("CurrentUserIdUsername");
            }

            return Redirect("/media");
        }

        [Route("/Users/{id:int}")]
        public IActionResult Show(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.MediaList = _context.Media.ToList();
            var user = _context.Users.Include(u => u.CurrentWatch).Include(u => u.AllWatched).Include(u => u.ToWatch)
            .FirstOrDefault(u => u.Id == id); ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Route("users/{id:int}/edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ViewBag.MediaList = _context.Media.ToList();
            var user = FindUserbyId(id);
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            ViewData["TakenError"] = TempData["TakenError"];

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [Route("users/{id:int}")]
        public IActionResult Update(int? id, User user)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentUser = FindUserbyId(id);

            if (currentUser == null)
            {
                return NotFound();
            }

            var usernames = _context.Users.Select(e => e.Username).ToList();
            var emails = _context.Users.Select(e => e.Email).ToList();

            usernames.Remove(currentUser.Username);
            emails.Remove(currentUser.Email);

            if (usernames.Any(e => e == user.Username))
            {
                TempData["TakenError"] = "That Username is already taken";
                return Redirect($"/users/{currentUser.Id}/edit");
            }
            if (emails.Any(e => e == user.Email))
            {
                TempData["TakenError"] = "That Email is already taken";
                return Redirect($"/users/{currentUser.Id}/edit");
            }
            else
            {
                currentUser.Age = user.Age;
                currentUser.Email = user.Email;
                currentUser.Name = user.Name;
                currentUser.Username = user.Username;
                try
                {
                    _context.Users.Update(currentUser);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when updating user to database");
                }

            }

            return Redirect($"/users/{currentUser.Id}");
        }

        [HttpPost]
        [Route("/Users/{id:int}/Delete")]
        public IActionResult Delete(int? id, string password)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.AllWatched)
                .Include(u => u.CurrentWatch)
                .Include(u => u.ToWatch)
                .First();
            if (user == null)
            {
                return NotFound();
            }

            if (password != null)
            {
                if (user.Password == user.ReturnEncryptedString(password))
                {
                    try
                    {
                        _context.Users.Remove(user);
                        _context.SaveChanges();
                        Response.Cookies.Delete("CurrentUserIdUsername");
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal("Error when deleting new user to database" + ex.Message);
                    }


                    return Redirect("/media");
                }
            }
            TempData["Incorrect"] = $"That password does not match the password for {user.Username}.";
            return Redirect($"/Users/{user.Id}/ConfirmPassword");
        }

        [Route("/Users/{id:int}/ConfirmPassword")]
        public IActionResult ConfirmPassword()
        {
            ViewData["CurrentUserIdUsername"] = Request.Cookies["CurrentUserIdUsername"];
            ViewData["Incorrect"] = TempData["Incorrect"];
            return View();
        }

        //jk: Add or remove from users allwatched list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/AllWatched")]
        public IActionResult AllWatched(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.AllWatched.Contains(movie))
            {
                try
                {
                    user.AllWatched.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.AllWatched.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when adding media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect($"/media/{movieId}");
        }

        //jk: Add or remove from users towatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/ToWatch")]
        public IActionResult ToWatch(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.ToWatch.Contains(movie))
            {
                try
                {
                    user.ToWatch.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.ToWatch.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect($"/media/{movieId}");
        }

        //jk: Add or remove from users CurrentlyWatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/CurrentlyWatch")]
        public IActionResult CurrentlyWatch(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.CurrentWatch.Contains(movie))
            {
                try
                {
                    user.CurrentWatch.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.CurrentWatch.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect($"/media/{movieId}");
        }

        [HttpPost]
        [Route("/Users/{id:int}/{mediaId:int}/CurrentlyWatch/Delete")]
        public IActionResult RemoveFromCurrentWatch(int Id, int mediaId)
        {
            // Retrieve the user and the media item
            var user = _context.Users.Where(u => u.Id == Id).Include(u => u.CurrentWatch).Single();
            var media = user.CurrentWatch.Where(m => m.Id == mediaId).FirstOrDefault();

            if (user.CurrentWatch.Contains(media))
            {
                try
                {
                    user.CurrentWatch.Remove(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            else
            {
                try
                {
                    user.CurrentWatch.Add(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Show", new { id = Id });
        }

        [HttpPost]
        [Route("/Users/{id:int}/{mediaId:int}/AllWatched/Delete")]
        public IActionResult RemoveFromAllWatched(int Id, int mediaId)
        {
            // Retrieve the user and the media item
            var user = _context.Users.Where(u => u.Id == Id).Include(u => u.AllWatched).Single();
            var media = user.AllWatched.Where(m => m.Id == mediaId).FirstOrDefault();

            if (user.AllWatched.Contains(media))
            {
                try
                {
                    user.AllWatched.Remove(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            else
            {
                try
                {
                    user.AllWatched.Add(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Show", new { id = Id });
        }

        [HttpPost]
        [Route("/Users/{id:int}/{mediaId:int}/ToWatch/Delete")]
        public IActionResult RemoveFromToWatch(int Id, int mediaId)
        {
            // Retrieve the user and the media item
            var user = _context.Users.Where(u => u.Id == Id).Include(u => u.ToWatch).Single();
            var media = user.ToWatch.Where(m => m.Id == mediaId).FirstOrDefault();

            if (user.ToWatch.Contains(media))
            {
                try
                {
                    user.ToWatch.Remove(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            else
            {
                try
                {
                    user.ToWatch.Add(media);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List");
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Show", new { id = Id });
        }

        //jk: Add or remove from users allwatched list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/Button/AllWatched")]
        public IActionResult AllWatchedButton(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.AllWatched.Contains(movie))
            {
                try
                {
                    user.AllWatched.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.AllWatched.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect("/media");
        }
        //jk: Add or remove from users towatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/Button/ToWatch")]
        public IActionResult ToWatchButton(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.ToWatch.Contains(movie))
            {
                try
                {
                    user.ToWatch.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.ToWatch.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect("/media");
        }
        //jk: Add or remove from users CurrentlyWatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/Button/CurrentlyWatch")]
        public IActionResult CurrentlyWatchButton(int id, int movieId)
        {
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            if (user.CurrentWatch.Contains(movie))
            {
                try
                {
                    user.CurrentWatch.Remove(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            else
            {
                try
                {
                    user.CurrentWatch.Add(movie);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Error when removing media from AllWatched List" + ex.Message);
                }
            }
            _context.SaveChanges();
            return Redirect("/media");
        }


        public User FindUserbyId(int? id)
        {
            return _context.Users.Find(id);

        }

        public Media FindMediaById(int? movieId)
        {
            return _context.Media.Find(movieId);
        }
    }
}
