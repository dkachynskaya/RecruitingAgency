using BLL.Contracts;
using BLL.DTOs;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork uow;
        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            List<UserDTO> users = AutoMapper.Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await uow.User.GetAll()).ToList();
            return users;
        }

        public async Task Update(UserDTO user)
        {
            if (user != null && user.Id > 0 && user.IsBlocked == false && user.FirstName.Length > 1 && user.LastName.Length > 1)
            {
                var updated = AutoMapper.Mapper.Map<UserDTO, User>(user);
                await uow.User.Update(updated);

                var newEmail = await uow.UserManager.FindByIdAsync(updated.Id);
                newEmail.Email = user.Email;
                await uow.UserManager.UpdateAsync(newEmail);
                await uow.Save();
            }
            else
                throw new ArgumentException("Wrong data");
        }

        public async Task Delete(int id)
        {
            List<Ad> userAds = new List<Ad>();
            userAds.AddRange(await uow.Ad.GetAll(x => x.UserId == id));
            foreach(var a in userAds)
            {
                await uow.Ad.Delete(a.Id);
            }

            await uow.User.Delete(id);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            return AutoMapper.Mapper.Map<User, UserDTO>(await uow.User.GetById(id));
        }

        public async Task<UserDTO> GetUserByLogin(string login)
        {
            return AutoMapper.Mapper.Map<User, UserDTO>((await uow.User.GetAll(x => x.Login == login)).FirstOrDefault());
        }

        public async Task<bool> IsLoginExist(string login)
        {
            return (await uow.User.GetAll(x => x.Login == login)).Any();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
