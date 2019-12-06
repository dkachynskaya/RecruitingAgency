using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IModeratorService: IDisposable
    {
        Task BlockAd(int adId, string login);
        Task UnblockAd(int adId);
    }
}
