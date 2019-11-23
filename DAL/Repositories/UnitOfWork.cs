using DAL.Contracts;
using DAL.EF;
using DAL.Identity;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;

        private GenericRepository<User> users;
        private GenericRepository<JobOffer> jobOffers;

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

        public IGenericRepository<JobOffer> JobOffer
        {
            get
            {
                if(jobOffers == null)
                {
                    jobOffers = new GenericRepository<JobOffer>(context);
                }
                return jobOffers;
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
