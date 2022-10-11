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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill{Id = 1,Name = "Fireball", Damage =30},
                new Skill{Id = 2,Name = "Water Magic", Damage =40},
                new Skill{Id = 3,Name = "Disaming Blow", Damage =20}
            );
        }
        // Contexts to be included in the Database and used by the API
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}