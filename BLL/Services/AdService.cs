using BLL.Contracts;
using BLL.DTOs;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BLL.Services
{
    public class AdService: IAdService
    {
        private readonly IUnitOfWork uow;
        public AdService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<AdDTO>> GetAllAds()
        {
            var ads = (await uow.Ad.GetAll()).OrderByDescending(x => x.CreateDate);

            if (ads == null)
                throw new ArgumentException("Not found");
            return Mapper.Map<IEnumerable<Ad>, IEnumerable<AdDTO>>(ads).ToList();
        }

        public async Task AddAd(AdDTO adDTO)
        {
            var ad = Mapper.Map<AdDTO, Ad>(adDTO);
            if (ad.PositionDescription != null)
            {
                await uow.Ad.Post(ad);
            }
            else
                throw new ArgumentException("Wrong data");
        }

        public async Task EditAd(AdDTO adDTO)
        {
            var ad = Mapper.Map<AdDTO, Ad>(adDTO);
            if (ad.UserId != 0)
            {
                await uow.Ad.Update(ad);
            }
            else
                throw new ArgumentException("Wrong data");
        }

        public async Task DeleteAd(int id)
        {
            await uow.Ad.Delete(id);
        }

        public async Task<AdDTO> GetAdById (int id)
        {
            var ad = await uow.Ad.GetById(id);
            if (ad == null)
                throw new ArgumentException("Not found");
            return Mapper.Map<Ad, AdDTO>(ad);
        }

        public async Task<IEnumerable<AdDTO>> GetAdsByUserId (int userId)
        {
            var ads = (await uow.Ad.GetAll(x => x.UserId == userId)).OrderByDescending(x => x.CreateDate).ToList();
            return Mapper.Map<IEnumerable<Ad>, IEnumerable<AdDTO>>(ads);
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
