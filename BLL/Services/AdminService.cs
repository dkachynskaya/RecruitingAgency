using BLL.Contracts;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AdminService: IAdminService
    {
        private readonly IUnitOfWork uow;
        public AdminService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task BlockUser(int userId, string login)
        {
            if (userId > 0 && login != null && login.Length > 3)
            {
                User user = await uow.User.GetById(userId);
                if (user.IsBlocked) throw new ArgumentException("User is already blocked!");
                user.IsBlocked = true;
                await uow.User.Update(user);
            }
            else throw new ArgumentException("Wrong data!");
        }

        public async Task UnblockUser(int userId)
        {
            if (userId > 0)
            {
                User user = await uow.User.GetById(userId);
                if (!user.IsBlocked) throw new ArgumentException("User is already unblocked");
                user.IsBlocked = false;
                await uow.User.Update(user);
            }
            else throw new ArgumentException("Wrong data");
        }

        public void Dispose()
        {
            uow?.Dispose();
        }
    }
}
