using Week5Lab.Models;
using Microsoft.EntityFrameworkCore;


namespace Week5Lab.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Class> Classes { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
