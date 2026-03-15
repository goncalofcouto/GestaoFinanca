using GestaoFinanca.Models;
using GestaoFinanca.Services;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanca.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<Users>().HasData([
                new Users { Id = 1, Name = "admin", Email = "admin@example.com", Password = PasswordHelper.HashPassword("admin123"), Role = UserRole.Admin }
            ]);
        }
        
    }
}