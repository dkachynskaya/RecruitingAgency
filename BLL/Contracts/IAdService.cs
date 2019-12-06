using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IAdService: IDisposable
    {
        Task<List<AdDTO>> GetAllAds();
        Task AddAd(AdDTO ad);
        Task EditAd(AdDTO ad);
        Task DeleteAd(int adId);
        Task<AdDTO> GetAdById(int adId);
        Task<IEnumerable<AdDTO>> GetAdsByUserId(int userid);
    }
}
