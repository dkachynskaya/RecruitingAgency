using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNet.Identity;
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
        [Route("block/ads/{adId}")]
        public async Task<IHttpActionResult> BlockAd([FromUri]int adId)
        {

            AdDTO ad = await uow.AdService.GetAdById(adId);
            if (ad == null)
                return NotFound();

            if (ad.IsBlocked)
                return BadRequest("Ad is already blocked.");

            var userName = User.Identity.GetUserName();

            if (userName == null || !User.IsInRole("Admin"))
                return this.Unauthorized();
            if (userName == null || !User.IsInRole("Moderator"))
                return this.Unauthorized();

            await uow.ModeratorService.BlockAd(adId, userName);

            return Ok("Ad is blocked");
        }

        [HttpGet]
        [Route("unblock/ads/{adId}")]
        public async Task<IHttpActionResult> UnblockAd([FromUri]int adId)
        {
            AdDTO ad = await uow.AdService.GetAdById(adId);
            if (ad == null)
                return NotFound();

            if (!ad.IsBlocked)
                return BadRequest("Ad is not blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("Admin"))
                return this.Unauthorized();


            await uow.ModeratorService.UnblockAd(adId);
            return Ok("Ad is unblocked");
        }
    }
}
