using Microsoft.EntityFrameworkCore;

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
    }
}
