using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IJobOfferService: IDisposable
    {
        Task<List<JobOfferDTO>> GetAllJobOffers();
        Task AddJobOffer(JobOfferDTO jobOffer);
        Task EditJobOffer(JobOfferDTO jobOffer);
        Task DeleteJobOffer(int jobOfferId);
        Task<JobOfferDTO> GetJobOfferById(int jobOfferId);
        Task<IEnumerable<JobOfferDTO>> GetJobOffersByUserId(int userid);
    }
}
