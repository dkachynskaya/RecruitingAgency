using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IModeratorService: IDisposable
    {
        Task BlockJobOffer(int jobOfferId, string login);
        Task UnblockJobOffer(int jobOfferId);
    }
}
