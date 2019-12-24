using System;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IModeratorService: IDisposable
    {
        Task BlockAd(int adId, string login);
        Task UnblockAd(int adId);
    }
}
