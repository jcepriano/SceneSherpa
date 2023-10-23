using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;
using System.Security.Cryptography;
using System.Text;
using Serilog;

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
            _context.Users.Add(user);
            _context.SaveChanges();

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
            string FailedLogin = "Either your password or username is incorrect, please try again.";
            if (ModelState.IsValid)
            {

                if (username != null && password != null)
                {
                    //Method FindUserByUsername
                    var LoginAttemptUser = FindUserbyUsername(username);

                    if (LoginAttemptUser != null)
                    {
                        //Method CheckHashedPassword
                        if (LoginAttemptUser.Password != null && CheckHashedPassword(password, LoginAttemptUser))
                        {
                            //Method AppendIdUsernameCookie
                            AppendIdUsernameCookie(LoginAttemptUser);
                            return Redirect($"/users/{LoginAttemptUser.Id}");
                        }
                        else
                        {
                            //Method FailedLoginTempData
                            TempData["FailedLogin"] = FailedLogin;
                        }

                    }
                    else
                    {
                        TempData["FailedLogin"] = FailedLogin;
                    }
                }

            }
            else if (username == null || password == null)
            {
                TempData["FailedLogin"] = FailedLogin;
            }
            else if (username == null && password == null)
            {
                TempData["FailedLogin"] = FailedLogin;
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
                _context.Users.Update(currentUser);
                _context.SaveChanges();
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
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    Response.Cookies.Delete("CurrentUserIdUsername");

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
            return GetObjectsThenToggleMedia(id, movieId, "AllWatched");
        }

        //jk: Add or remove from users towatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/ToWatch")]
        public IActionResult ToWatch(int id, int movieId)
        {
            return GetObjectsThenToggleMedia(id, movieId, "ToWatch");
        }

        //jk: Add or remove from users CurrentlyWatch list
        [HttpPost]
        [Route("/Users/{id:int}/{movieId:int}/CurrentlyWatch")]
        public IActionResult CurrentlyWatch(int id, int movieId)
        {
            return GetObjectsThenToggleMedia(id, movieId, "CurrentlyWatch");
        }

        private IActionResult GetObjectsThenToggleMedia(int id, int movieId, string listName)
        {
            string referer = Request.Headers.Referer;
            var user = FindUserbyId(id);
            var movie = FindMediaById(movieId);
            ToggleMediaInList(user, movie, listName);

            if (string.IsNullOrEmpty(referer))
            {
                return NotFound();
            }
            return Redirect(referer);
        }

        private void ToggleMediaInList(User user, Media media, string listName)
        {
            List<Media> targetList = user.GetListFromName(listName);

            if(targetList != null)
            {
                if (targetList.Contains(media))
                {
                    try
                    {
                        targetList.Remove(media);
                    }
                    catch(Exception ex)
                    {
                        Log.Fatal("Failed to remove media from list" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        targetList.Add(media);
                    }
                    catch(Exception ex)
                    {
                        Log.Fatal("Failed to Add media to list" + ex.Message);
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                TempData["TargetListError"] = "There was an error processing this request";
            }
        }

        public User FindUserbyId(int? id)
        {
            return _context.Users.Find(id);

        }

        public User FindUserbyUsername(string username)
        {

            return _context.Users.Where(e => e.Username == username).FirstOrDefault();
        }
        public bool CheckHashedPassword(string password, User LoginAttemptUser)
        {
            return LoginAttemptUser.Password == LoginAttemptUser.ReturnEncryptedString(password);
        }
        public void AppendIdUsernameCookie(User user)
        {
            Response.Cookies.Append("CurrentUserIdUsername", $"{user.Id} {user.Username}");
        }

        public Media FindMediaById(int? movieId)
        {
            return _context.Media.Find(movieId);
        }

        public string SetTempData()
        {
            return "Either your password or username is incorrect, please try again.";
        }
    }
}
