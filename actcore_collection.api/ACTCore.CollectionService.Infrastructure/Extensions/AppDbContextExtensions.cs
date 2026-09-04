using ACTCore.CollectionService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ACTCore.CollectionService.API.Data.Extensions
{
    public static class AppDbContextExtensions
    {
        public static void ConfigurationIdentityTable(this AppDbContext context, ModelBuilder modelBuilder)
        {
            // Add more configurations or queries as needed
            //modelBuilder.Entity<ApplicationUser>()
            //    .ToTable("Users");
            //modelBuilder.Entity<IdentityRole>()
            //    .ToTable("UserRoles");
            //modelBuilder.Entity<IdentityUserRole<string>>()
            //    .ToTable("UserUserRoles");
            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .ToTable("UserClaims");
            //modelBuilder.Entity<IdentityUserLogin<string>>()
            //    .ToTable("UserLogins");
            //modelBuilder.Entity<IdentityUserToken<string>>()
            //    .ToTable("UserTokens");
            //modelBuilder.Entity<IdentityRoleClaim<string>>()
            //    .ToTable("UserRoleClaims");
        }
    }
}
