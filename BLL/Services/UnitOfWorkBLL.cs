using BLL.Contracts;
using DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UnitOfWorkBLL: IUnitOfWorkBLL
    {
        private readonly IUnitOfWork uow;
        private AdminService adminService;
        private AdService adService;
        private CategoryService categoryService;
        private ModeratorService moderatorService;
        private UserManagerService userManagerService;
        private UserService userService;

        public UnitOfWorkBLL(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IAdminService AdminService
        {
            get
            {
                if(adminService == null)
                {
                    adminService = new AdminService(uow);
                }
                return adminService;
            }
        }

        public IModeratorService ModeratorService
        {
            get
            {
                if (moderatorService == null)
                {
                    moderatorService = new ModeratorService(uow);
                }
                return moderatorService;
            }
        }

        public IUserManagerService UserManagerService
        {
            get
            {
                if (userManagerService == null)
                {
                    userManagerService = new UserManagerService(uow);
                }
                return userManagerService;
            }
        }

        public IUserService UserService
        {
            get
            {
                if (userService == null)
                {
                    userService = new UserService(uow);
                }
                return userService;
            }
        }

        public IAdService AdService
        {
            get
            {
                if (adService == null)
                {
                    adService = new AdService(uow);
                }
                return adService;
            }
        }

        public ICategoryService CategoryService
        {
            get
            {
                if (categoryService == null)
                {
                    categoryService = new CategoryService(uow);
                }
                return categoryService;
            }
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    uow?.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
