using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Models.Database
{
    // Klasa kontekstu bazy danych dziedzicząca po IdentityDbContext, składająca się również z postów i tematów 
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {}
        public DbSet<Post> Post { get; set; }
        public DbSet<Topic> Topic { get; set; }
    }
}
