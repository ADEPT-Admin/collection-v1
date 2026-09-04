using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Data.DbContexts
{
    public class ExampleAppDbContext : DbContext
    {
        public ExampleAppDbContext(DbContextOptions<ExampleAppDbContext> options) : base(options) { }

        public DbSet<ExampleEntity> ExampleEntities { get; set; }

        // ...OnModelCreating, etc...
    }
}