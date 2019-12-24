using System;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IAdminService: IDisposable
    {
        Task BlockUser(int userId, string login);
        Task UnblockUser(int userId);
    }
}
