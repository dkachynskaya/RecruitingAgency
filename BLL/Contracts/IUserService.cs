using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IUserService: IDisposable
    {
        Task<List<UserDTO>> GetAllUsers();
        Task Update(UserDTO user);
        Task Delete(int userId);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> GetUserByLogin(string login);
        Task<bool> IsLoginExist(string login);
    }
}
