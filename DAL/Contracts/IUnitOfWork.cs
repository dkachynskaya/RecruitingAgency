using DAL.Identity;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUnitOfWork: IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IGenericRepository<User> User { get; }
        IGenericRepository<Ad> Ad { get; }
        IGenericRepository<Category> Category { get; }

        Task Save();
    }
}
