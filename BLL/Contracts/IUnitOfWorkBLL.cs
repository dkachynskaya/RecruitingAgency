using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IUnitOfWorkBLL
    {
        IAdminService AdminService { get; }
        ICategoryService CategoryService { get; }
        IAdService AdService { get; }
        IModeratorService ModeratorService { get; }
        IUserManagerService UserManagerService { get; }
        IUserService UserService { get; }
    }
}
