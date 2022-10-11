using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Data
{
    /// <summary>
    /// Custom class that inherits from the Entity Framework DBContext
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base (options)
        { }
        // Contexts to be included in the Database and used by the API
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}