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
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admins")]
    public class AdminController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public AdminController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [Route("block/user/{accountLogin}")]
        public async Task<IHttpActionResult> BlockAccount([FromUri]string accountLogin)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            UserDTO user = await uow.UserManagerService.GetUserByLogin(accountLogin);
            if (user == null)
                return NotFound();

            if (user.IsBlocked)
                return BadRequest("User already blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("Admin"))
                return this.Unauthorized();

            var admin = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            if (admin != null)
                await uow.AdminService.BlockUser(user.Id, admin.Login);
            else
                return BadRequest("Not found ");

            return Ok("Account blocked");
        }

        [HttpGet]
        [Route("unblock/user/{accountLogin}")]
        public async Task<IHttpActionResult> UnblockAccount([FromUri]string accountLogin)
        {
            UserDTO user = await uow.UserManagerService.GetUserByLogin(accountLogin);
            if (user == null)
                return NotFound();
            if (!user.IsBlocked)
                return BadRequest("User is not blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("Admin"))
                return this.Unauthorized();

            await uow.AdminService.UnblockUser(user.Id);
            return Ok("Account unblocked");
        }
    }
}
