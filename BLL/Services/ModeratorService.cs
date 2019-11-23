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

        public async Task BlockJobOffer(int jobOfferId, string login)
        {
            if (jobOfferId > 0 && login != null && login.Length > 3)
            {
                JobOffer jobOffer = await uow.JobOffer.GetById(jobOfferId);
                jobOffer.IsActual = true;
                await uow.JobOffer.Update(jobOffer);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task UnblockJobOffer(int jobOfferId)
        {
            if (jobOfferId > 0)
            {
                JobOffer jobOffer = await uow.JobOffer.GetById(jobOfferId);
                jobOffer.IsActual = false;
                await uow.JobOffer.Update(jobOffer);
            }
            else throw new ArgumentException("Wrong data");
        }

        public void Dispose()
        {
            uow?.Dispose();
        }

    }
}
