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
    public class AdController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public AdController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ads")]
        public async Task<IHttpActionResult> GetAllAds()
        {
            var ads = (await uow.AdService.GetAllAds()).ToList();
            ads.RemoveAll(x => x.IsBlocked == true);      
            if (ads == null)
                NotFound();  //code 404         

            IEnumerable<AdViewModel> Ads = AutoMapper.Mapper.Map<IEnumerable<AdDTO>, List<AdViewModel>>(ads);
            return Ok(Ads);
        }

        [HttpGet]
        [Route("{adId}")]
        public async Task<IHttpActionResult> GetAdById(int adId)
        {
            var ad = (await uow.AdService.GetAdById(adId));
            if (ad == null)
                NotFound();  //code 404         
            AdViewModel Ad = AutoMapper.Mapper.Map<AdDTO, AdViewModel>(ad);
            return Ok(Ad);
        }

        [HttpGet]
        [Route("getalladsModer")]
        public async Task<IHttpActionResult> GetAllAdsModer()
        {
            var ads = (await uow.AdService.GetAllAds()).ToList();
            if (ads == null)
                NotFound();  //code 404         

            IEnumerable<AdViewModel> Ads = AutoMapper.Mapper.Map<IEnumerable<AdDTO>, List<AdViewModel>>(ads);
            return Ok(Ads);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddAd()
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            var httpRequest = HttpContext.Current.Request;  //get request object   

            string positionDescriptionCode = httpRequest.Params["Ad"];
            string positionDescription = Base64Decode(positionDescriptionCode); //decoding string with html tag

            AdDTO ad = new AdDTO
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

            await uow.AdService.AddAd(ad);
            return Content(HttpStatusCode.Created, "Ad is added");
        }

        [HttpPut]
        [Route("{adId}")]
        public async Task<IHttpActionResult> EditAd([FromUri]int adId, [FromBody] AdEditViewModel newAd)
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account is blocked.");

            AdDTO ad = await uow.AdService.GetAdById(adId);
            if (ad == null)
                return NotFound();

            if (ad.User.Id != authtor.Id)
                return BadRequest("It is not your post.");

            if (ad.IsBlocked)
                return BadRequest("As is blocked.");

            ad.PositionName = newAd.PositionName;
            ad.Location = newAd.Location;
            ad.Company = newAd.Company;
            ad.PositionDescription = newAd.PositionDescription;
            ad.User = null;

            await uow.AdService.EditAd(ad);
            return Ok("Ad is edited");
        }

        [HttpDelete]
        [Route("{adId}")]
        public async Task<IHttpActionResult> DeleteAd([FromUri]int adId)
        {
            var authtor = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            AdDTO ad = await uow.AdService.GetAdById(adId);
            if (ad == null)
                return NotFound();

            if (ad.User.Id != authtor.Id)
                return BadRequest("It is not your post.");

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            await uow.AdService.DeleteAd(adId);
            return Ok("Ad is deleted");
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
