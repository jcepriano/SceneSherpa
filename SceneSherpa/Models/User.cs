using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SceneSherpa.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(25, ErrorMessage = "Name cannot be more than 25 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(25, ErrorMessage = "Username cannot be more than 25 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email cannot be more than 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StrongPassword]
        public string Password { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(13, 1000)]
        public int Age { get; set; }

        public List<Media>? CurrentWatch { get; set; } = new List<Media>();
        public List<Media>? AllWatched { get; set; } = new List<Media>();
        public List<Media>? ToWatch { get; set; } = new List<Media>();
        public List<User>? Friends { get; set; } = new List<User>();
        public List<User>? Friended { get; set; } = new List<User>();


        public List<Media> GetListFromName(string listName)
        {
            if (listName == "ToWatch") return ToWatch;
            if (listName == "AllWatched") return AllWatched;
            if (listName == "CurrentlyWatch") return CurrentWatch;

            else
            {
                Log.Warning($"Failed to retrieve desired list {listName}");
                return null;
            }
        }
        //this method will take in any string and return it hashed. I've been using it for personal User information.
        public string ReturnEncryptedString(string stringToEncrypt)
        {
            HashAlgorithm sha = SHA256.Create();
            byte[] firstInputBytes = Encoding.ASCII.GetBytes(stringToEncrypt);
            byte[] firstInputDigested = sha.ComputeHash(firstInputBytes);

            StringBuilder firstInputBuilder = new StringBuilder();
            foreach (byte b in firstInputDigested)
            {
                Console.Write(b + ", ");
                firstInputBuilder.Append(b.ToString("x2"));
            }
            return firstInputBuilder.ToString();
        }


    }
}