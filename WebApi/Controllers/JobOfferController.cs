using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models.JobOffer;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/jobOffers")]
    public class JobOfferController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public JobOfferController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getalljoboffers")]
        public async Task<IHttpActionResult> GetAllJobOffers()
        {

            var jobOffers = (await uow.JobOfferService.GetAllJobOffers()).ToList();
            jobOffers.RemoveAll(x => x.IsActual == true);      
            if (jobOffers == null)
                NotFound();  //code 404         

            IEnumerable<JobOfferViewModel> JobOffers = AutoMapper.Mapper.Map<IEnumerable<JobOfferDTO>, List<JobOfferViewModel>>(jobOffers);
            return Ok(JobOffers);
        }

        [HttpGet]
        [Route("{jobOffersId}")]
        public async Task<IHttpActionResult> GetJobOfferById(int jobOfferId)
        {
            var jobOffer = (await uow.JobOfferService.GetJobOfferById(jobOfferId));
            if (jobOffer == null)
                NotFound();  //code 404         
            JobOfferViewModel JobOffer = AutoMapper.Mapper.Map<JobOfferDTO, JobOfferViewModel>(jobOffer);
            return Ok(JobOffer);
        }

        [HttpGet]
        [Route("getalljobOfferModer")]
        public async Task<IHttpActionResult> GetAllJobOfferModer()
        {
            var jobOffers = (await uow.JobOfferService.GetAllJobOffers()).ToList();
            if (jobOffers == null)
                NotFound();  //code 404         

            IEnumerable<JobOfferViewModel> JobOffers = AutoMapper.Mapper.Map<IEnumerable<JobOfferDTO>, List<JobOfferViewModel>>(jobOffers);
            return Ok(JobOffers);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddJobOffer()
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            var httpRequest = HttpContext.Current.Request;  //get request object   

            string positionDescriptionCode = httpRequest.Params["JobOffer"];
            string positionDescription = Base64Decode(positionDescriptionCode); //decoding string with html tag

            JobOfferDTO jO = new JobOfferDTO
            {
                PositionName = httpRequest.Params["PositionName"],
                Location = httpRequest.Params["Location"],
                Company = httpRequest.Params["Company"],
                PositionDescription = positionDescription,
                CreateDate = DateTime.Now,
                UserId = authtor.Id,
            };

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            await uow.JobOfferService.AddJobOffer(jO);
            return Content(HttpStatusCode.Created, "JobOffer is added");
        }

        [HttpPut]
        [Route("{jobOfferId}")]
        public async Task<IHttpActionResult> EditJobOffer([FromUri]int jobOfferId, [FromBody] JobOfferEditViewModel newJobOffer)
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account is blocked.");

            JobOfferDTO jobOffer = await uow.JobOfferService.GetJobOfferById(jobOfferId);
            if (jobOffer == null)
                return NotFound();

            if (jobOffer.User.Id != authtor.Id)
                return BadRequest("It is not your post.");

            if (jobOffer.IsActual)
                return BadRequest("JobOffer is blocked.");

            jobOffer.PositionName = newJobOffer.PositionName;
            jobOffer.Location = newJobOffer.Location;
            jobOffer.Company = newJobOffer.Company;
            jobOffer.PositionDescription = newJobOffer.PositionDescription;
            jobOffer.User = null;

            await uow.JobOfferService.EditJobOffer(jobOffer);
            return Ok("JobOffer is edited");
        }

        [HttpDelete]
        [Route("{jobOfferId}")]
        public async Task<IHttpActionResult> DeleteJobOffer([FromUri]int jobOfferId)
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            JobOfferDTO jobOffer = await uow.JobOfferService.GetJobOfferById(jobOfferId);
            if (jobOffer == null)
                return NotFound();

            if (jobOffer.User.Id != authtor.Id)
                return BadRequest("It is not your post.");

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            await uow.JobOfferService.DeleteJobOffer(jobOfferId);
            return Ok("JobOffer is deleted");
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
