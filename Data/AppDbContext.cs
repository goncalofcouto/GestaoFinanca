using GestaoFinanca.Models;
using GestaoFinanca.Services;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanca.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}

        public DbSet<Users> DbGestaoFinanca { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<Users>().HasData([
                new Users { Id = 1, Name = "admin", Email = "admin@example.com", PasswordHash = PasswordHelper.HashPassword("admin123") }
            ]);
        }
        
    }
}