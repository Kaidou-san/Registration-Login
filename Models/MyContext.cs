using Microsoft.EntityFrameworkCore;

namespace LogRegJune
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        // dont forget to add ur DBSet here!!
        public DbSet<User> Users {get; set;}
    }
}