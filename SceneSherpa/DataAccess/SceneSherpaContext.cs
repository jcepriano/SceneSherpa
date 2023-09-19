using Microsoft.EntityFrameworkCore;

namespace SceneSherpa.DataAccess
{
    public class SceneSherpaContext : DbContext
    {
        //Dbsets
        public SceneSherpaContext(DbContextOptions<SceneSherpaContext> options) : base(options)
        {

        }
    }
}
