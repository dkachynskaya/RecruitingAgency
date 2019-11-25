using BLL.Contracts;
using BLL.DTOs;
using BLL.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models.Account;

namespace WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        private IAuthenticationManager authenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private readonly IUnitOfWorkBLL uow;

        public AccountController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [Authorize]
        [HttpGet]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            authenticationManager.SignOut();
            return Ok(User.Identity.Name + " logged out");
        }

        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest(User.Identity.Name + " already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data.");
            }

            if (await uow.UserService.CheckLoginExist(model.Login))
            {
                ModelState.AddModelError("Login", "Login is already taken.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            UserDTO user = AutoMapper.Mapper.Map<RegisterViewModel, UserDTO>(model);
            user.Roles = new List<string> { "User" };  
            user.RegistrationDate = DateTime.Now;

            OperationDetails operationDetails = await uow.UserManagerService.Create(user);
            if (!operationDetails.Success)
            {
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                return this.BadRequest(operationDetails.Message);
            }
            else
            {
                return this.Ok(user.Login + " user registered ok.");
            }
        }
    }
}
