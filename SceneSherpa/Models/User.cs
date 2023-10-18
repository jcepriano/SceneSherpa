using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SceneSherpa.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(747, ErrorMessage = "Name cannot exceed 747 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Age is Required")]
        public int Age { get; set; }

        public List<Media>? CurrentWatch { get; set; } = new List<Media>();
        public List<Media>? AllWatched { get; set; } = new List<Media>();
        public List<Media>? ToWatch { get; set; } = new List<Media>();
        public List<User>? Friends { get; set; } = new List<User>();
        public List<User>? Friended { get; set; } = new List<User>();


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