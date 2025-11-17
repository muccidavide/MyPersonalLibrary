using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Configurations;

namespace MyPersonalLibrary.Server.Models.Context
{
    public class MyPersonalLibraryContext : DbContext
    {
        public MyPersonalLibraryContext(DbContextOptions options) : base(options)
        {
        }

        protected MyPersonalLibraryContext()
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

         protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applica tutte le configurazioni Fluent API
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
    }
    }
}
