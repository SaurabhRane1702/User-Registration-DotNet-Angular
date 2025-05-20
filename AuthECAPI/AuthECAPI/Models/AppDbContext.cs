using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthECAPI.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<TimeTable> TimeTables { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<LibraryCard> LibraryCards { get; set; }
    }
}
