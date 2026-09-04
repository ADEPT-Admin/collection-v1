using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ACTCore.CollectionService.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>, IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Configure your connection string here
            optionsBuilder.UseSqlServer("YourConnectionString");
            return new AppDbContext(optionsBuilder.Options);
        }

        // For IDbContextFactory
        AppDbContext IDbContextFactory<AppDbContext>.CreateDbContext()
        {
            return CreateDbContext();
        }
    }
}