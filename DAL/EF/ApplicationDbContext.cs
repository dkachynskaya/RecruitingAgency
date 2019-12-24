using DAL.Identity;
using DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DAL.EF
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin,
        CustomUserRole, CustomUserClaim>
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer(new ContextInitializer());
        }

        public ApplicationDbContext(string connection)
            : base(connection)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>().HasRequired(i => i.User).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
