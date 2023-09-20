using Microsoft.EntityFrameworkCore;
using SceneSherpa.Models;

namespace SceneSherpa.DataAccess
{
    public class SceneSherpaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public SceneSherpaContext(DbContextOptions<SceneSherpaContext> options) : base(options)
        {

        }
    }
}
