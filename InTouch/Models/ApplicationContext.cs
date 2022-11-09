using Microsoft.EntityFrameworkCore;

namespace InTouch.Models
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=InTouchDB;Trusted_Connection=True;");
        }
    }
}
