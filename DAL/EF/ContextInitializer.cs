using DAL.Identity;
using DAL.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class ContextInitializer: CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new CustomUserStore(context));

            var roleManager = new ApplicationRoleManager(new CustomRoleStore(context));

            var role1 = new CustomRole("User");
            var role2 = new CustomRole("Admin");
            var role3 = new CustomRole("Moderator");

            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new ApplicationUser()
            {
                Email = "daria@gmail.com",
                UserName = "Daria",
                User = new User()
                {
                    FirstName = "Daria",
                    LastName = "Kachynska",
                    Login = "DariaK",
                    IsBlocked = false,
                    RegistrationDate = DateTime.Now
                }
            };
            var result = userManager.Create(admin, "1234567890");

            var user = new ApplicationUser()
            {
                Email = "sergey@ukr.net",
                UserName = "Sergey",
                User = new User()
                {
                    FirstName = "Sergey",
                    LastName = "Korolyov",
                    Login = "Serg",
                    IsBlocked = false,
                    RegistrationDate = DateTime.Now
                }
            };
            var result2 = userManager.Create(user, "0987654321");

            var moderator = new ApplicationUser()
            {
                Email = "moderator@gmail.com",
                UserName = "Oleg",
                User = new User()
                {
                    FirstName = "Oleg",
                    LastName = "Marchenko",
                    Login = "Oleg",
                    IsBlocked = false,
                    RegistrationDate = DateTime.Now
                }
            };
            var result3 = userManager.Create(moderator, "qwerty123");
            context.SaveChanges();

            var cat = new Category()
            {
                Name = "IT"
            };
            context.Categories.Add(cat);
            context.SaveChanges();

            var ad = new Ad()
            {
                PositionName = "Waiter",
                Location = "Kiev",
                Company = "Cafe",
                PositionDescription = "Description",
                UserId = 1,
                CreateDate = DateTime.Now,
                CategoryId = 1
            };
            context.Ads.Add(ad);
            context.SaveChanges();

            if(result.Succeeded && result2.Succeeded && result3.Succeeded)
            {
                userManager.AddToRole(admin.Id, role2.Name);
                userManager.AddToRole(admin.Id, role3.Name);
                userManager.AddToRole(user.Id, role1.Name);
                userManager.AddToRole(moderator.Id, role3.Name);
            }
            context.SaveChanges();
        }
    }
}
