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
    public class ModeratorService: IModeratorService
    {
        private readonly IUnitOfWork uow;
        public ModeratorService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task BlockAd(int adId, string login)
        {
            if (adId > 0 && login != null && login.Length > 3)
            {
                Ad ad = await uow.Ad.GetById(adId);
                ad.IsBlocked = true;
                await uow.Ad.Update(ad);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task UnblockAd(int adId)
        {
            if (adId > 0)
            {
                Ad ad = await uow.Ad.GetById(adId);
                ad.IsBlocked = false;
                await uow.Ad.Update(ad);
            }
            else throw new ArgumentException("Wrong data");
        }

        public void Dispose()
        {
            uow?.Dispose();
        }

    }
}
