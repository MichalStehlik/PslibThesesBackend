using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
        }
    }
}
