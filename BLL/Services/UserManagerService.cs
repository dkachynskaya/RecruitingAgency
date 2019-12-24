using BLL.Contracts;
using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Contracts;
using DAL.Identity;
using DAL.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManagerService: IUserManagerService
    {
        private readonly IUnitOfWork uow;
        public UserManagerService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> CheckUser(UserDTO userDTO)
        {
            ApplicationUser user = await uow.UserManager.FindAsync(userDTO.Login, userDTO.Password);
            return await uow.UserManager.CheckPasswordAsync(user, userDTO.Password);
        }

        public async Task<OperationDetails> Create(UserDTO userDTO)
        {
            ApplicationUser user = await uow.UserManager.FindByEmailAsync(userDTO.Email); 
            if (user == null)
            {
                user = AutoMapper.Mapper.Map<UserDTO, ApplicationUser>(userDTO);
                user.User = AutoMapper.Mapper.Map<UserDTO, User>(userDTO);


                var result = await uow.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await uow.UserManager.AddToRoleAsync(user.Id, userDTO.Roles[0]);   
                await uow.Save();
                return new OperationDetails(true, "Registration success", "");
            }
            else
            {
                return new OperationDetails(false, "User with this email exist", "email");
            }
        }

        public async Task<ClaimsIdentity> GetClaims(string username, string password)
        {
            var user = await uow.UserManager.FindAsync(username, password);

            if (user != null)
            {
                ClaimsIdentity claim = await uow.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ExternalBearer); 
                return claim;
            }
            else return null;
        }

        public async Task ChangePassword(int userId, string password, string newPassword)
        {
            await uow.UserManager.ChangePasswordAsync(userId, password, newPassword);
        }

        public async Task<UserDTO> GetUserByLogin(string login)
        {
            return AutoMapper.Mapper.Map<ApplicationUser, UserDTO>(await uow.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == login));
        }

        public async Task<bool> IsUserInRoleAdmin(int userId)
        {
            return await uow.UserManager.IsInRoleAsync(userId, "Admin");
        }

        public async Task<bool> IsUserInRoleModerator(int userId)
        {
            return await uow.UserManager.IsInRoleAsync(userId, "Moderator");
        }
    }
}
