﻿using Microsoft.EntityFrameworkCore;
using SceneSherpa.Models;

namespace SceneSherpa.DataAccess
{
    public class SceneSherpaContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Media> Media { get; set; }
        DbSet<Review> Reviews { get; set; }

        public SceneSherpaContext(DbContextOptions<SceneSherpaContext> options) : base(options)
        {

        }
    }
}
