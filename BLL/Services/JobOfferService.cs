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
    public class JobOfferService: IJobOfferService
    {
        private readonly IUnitOfWork uow;
        public JobOfferService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<JobOfferDTO>> GetAllJobOffers()
        {
            var jobOffers = (await uow.JobOffer.GetAll()).OrderByDescending(x => x.CreateDate);

            if (jobOffers == null)
                throw new ArgumentException("Not found");
            return Mapper.Map<IEnumerable<JobOffer>, IEnumerable<JobOfferDTO>>(jobOffers).ToList();
        }

        public async Task AddJobOffer(JobOfferDTO jobOfferDTO)
        {
            var jobOffer = Mapper.Map<JobOfferDTO, JobOffer>(jobOfferDTO);
            if (jobOffer.PositionDescription != null)
            {
                await uow.JobOffer.Post(jobOffer);
            }
            else
                throw new ArgumentException("Wrong data");
        }

        public async Task EditJobOffer(JobOfferDTO jobOfferDTO)
        {
            var jobOffer = Mapper.Map<JobOfferDTO, JobOffer>(jobOfferDTO);
            if (jobOffer.UserId != 0)
            {
                await uow.JobOffer.Update(jobOffer);
            }
            else
                throw new ArgumentException("Wrong data");
        }

        public async Task DeleteJobOffer(int id)
        {
            await uow.JobOffer.Delete(id);
        }

        public async Task<JobOfferDTO> GetJobOfferById (int id)
        {
            var jobOffer = await uow.JobOffer.GetById(id);
            if (jobOffer == null)
                throw new ArgumentException("Not found");
            return Mapper.Map<JobOffer, JobOfferDTO>(jobOffer);
        }

        public async Task<IEnumerable<JobOfferDTO>> GetJobOffersByUserId (int userId)
        {
            var jobOffers = (await uow.JobOffer.GetAll(x => x.UserId == userId)).OrderByDescending(x => x.CreateDate).ToList();
            return Mapper.Map<IEnumerable<JobOffer>, IEnumerable<JobOfferDTO>>(jobOffers);
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
