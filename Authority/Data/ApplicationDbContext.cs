using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Authority.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Authority.ViewModels.ApplicationUserViewModel> ApplicationUserViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "ADMIN", Name = "Administrátor" });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "MANAGER", Name = "Manažer" });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "TEACHER", Name = "Učitel" });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "STUDENT", Name = "Student" });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "EXT", Name = "Externista" });
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "ADMINUSER",
                FirstName = "Hlavní",
                LastName = "Administrátor",
                MiddleName = "",
                Gender = Gender.Unknown,
                Email = "st@pslib.cloud",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "Admin",
                PasswordHash = hasher.HashPassword(null,"Admin_1234"),
                SecurityStamp = string.Empty
            });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "ADMIN", UserId = "ADMINUSER" });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(new IdentityRoleClaim<string> { Id = 1, RoleId = "ADMIN", ClaimType = "admin", ClaimValue = "1"});
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(new IdentityRoleClaim<string> { Id = 2, RoleId = "TEACHER", ClaimType = "theses_evaluator", ClaimValue = "1" });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(new IdentityRoleClaim<string> { Id = 3, RoleId = "EXT", ClaimType = "theses_evaluator", ClaimValue = "1" });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(new IdentityRoleClaim<string> { Id = 4, RoleId = "MANAGER", ClaimType = "theses_manager", ClaimValue = "1" });
        }
    }
}
