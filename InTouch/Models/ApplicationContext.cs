using InTouch.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Windows;

namespace InTouch.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
