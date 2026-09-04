using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SharedKernel.Data.DbContexts
{
    public class ExampleAppDbContextFactory : IDesignTimeDbContextFactory<ExampleAppDbContext>, IDbContextFactory<ExampleAppDbContext>
    {
        public ExampleAppDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExampleAppDbContext>();
            // Configure your connection string here
            optionsBuilder.UseSqlServer("YourConnectionString");
            return new ExampleAppDbContext(optionsBuilder.Options);
        }

        // For IDbContextFactory
        ExampleAppDbContext IDbContextFactory<ExampleAppDbContext>.CreateDbContext()
        {
            return CreateDbContext();
        }
    }
}