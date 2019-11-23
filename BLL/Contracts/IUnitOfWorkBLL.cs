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
        IJobOfferService JobOfferService { get; }
        IModeratorService ModeratorService { get; }
        IUserManagerService UserManagerService { get; }
        IUserService UserService { get; }
    }
}
