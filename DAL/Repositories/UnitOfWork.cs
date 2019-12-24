using DAL.Contracts;
using DAL.EF;
using DAL.Identity;
using DAL.Models;
using System;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;

        private GenericRepository<User> users;
        private GenericRepository<Ad> ads;
        private GenericRepository<Category> categories;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;

        public UnitOfWork(string connection)
        {
            context = new ApplicationDbContext(connection);
        }

        public IGenericRepository<User> User
        {
            get
            {
                if(users == null)
                {
                    users = new GenericRepository<User>(context);
                }
                return users;
            }
        }

        public IGenericRepository<Ad> Ad
        {
            get
            {
                if(ads == null)
                {
                    ads = new GenericRepository<Ad>(context);
                }
                return ads;
            }
        }

        public IGenericRepository<Category> Category
        {
            get
            {
                if (categories == null)
                {
                    categories = new GenericRepository<Category>(context);
                }
                return categories;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (userManager == null)
                {
                    userManager = new ApplicationUserManager(new CustomUserStore(context));
                }
                return userManager;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if(roleManager == null)
                {
                    roleManager = new ApplicationRoleManager(new CustomRoleStore(context));
                }
                return roleManager;
            }
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                    roleManager.Dispose();
                    userManager.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
