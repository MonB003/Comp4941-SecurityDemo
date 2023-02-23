using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace User.Management.API.Models
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
		// DBContext is responsible for interacting with data as objects
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

		// Constructs the database schema
		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        // Sets up roles in the database
        private static void SeedRoles(ModelBuilder builder)
        {
            // In this code, there are 3 roles: Admin, User, and HR
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
                new IdentityRole() { Name = "HR", ConcurrencyStamp = "3", NormalizedName = "HR" }
                );
        }
    }
}
