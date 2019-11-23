using BLL.Contracts;
using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Moderator, Admin")]
    [RoutePrefix("api/moderator")]
    public class ModeratorController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public ModeratorController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [Route("block/jobOffers/{jobOfferId}")]
        public async Task<IHttpActionResult> BlockJobOffer([FromUri]int jobOfferId)
        {

            JobOfferDTO jobOffer = await uow.JobOfferService.GetJobOfferById(jobOfferId);
            if (jobOffer == null)
                return NotFound();

            if (jobOffer.IsActual)
                return BadRequest("JobOffer is already blocked.");

            var userName = User.Identity.GetUserName();

            if (userName == null || !User.IsInRole("Admin"))
                return this.Unauthorized();
            if (userName == null || !User.IsInRole("Moderator"))
                return this.Unauthorized();

            await uow.ModeratorService.BlockJobOffer(jobOfferId, userName);

            return Ok("JobOffer is blocked");
        }

        [HttpGet]
        [Route("unblock/jobOffer/{jobOfferId}")]
        public async Task<IHttpActionResult> UnblockJobOffer([FromUri]int jobOfferId)
        {
            JobOfferDTO jobOffer = await uow.JobOfferService.GetJobOfferById(jobOfferId);
            if (jobOffer == null)
                return NotFound();

            if (!jobOffer.IsActual)
                return BadRequest("JobOffer is not blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("Admin"))
                return this.Unauthorized();


            await uow.ModeratorService.UnblockJobOffer(jobOfferId);
            return Ok("JobOffer is unblocked");
        }
    }
}
