using BLL.DTOs;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IUserManagerService
    {
        Task<bool> CheckUser(UserDTO user);
        Task<OperationDetails> Create(UserDTO user);
        Task<ClaimsIdentity> GetClaims(string username, string password);
        Task<UserDTO> GetUserByLogin(string login);
        Task<bool> IsUserInRoleAdmin(int userId);
        Task<bool> IsUserInRoleModerator(int userId);
        Task ChangePassword(int userId, string password, string newPassword);
    }
}
