using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.JobOffer;
using WebApi.Models.User;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUnitOfWorkBLL uow;

        public UserController(IUnitOfWorkBLL uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IHttpActionResult> MyProfile()
        {
            var profile = await uow.UserService.GetUserById(User.Identity.GetUserId<int>());
            var emailData = await uow.UserManagerService.GetUserByLogin(profile.Login);
            profile.Email = emailData.Email;

            if (profile == null)
                return this.Unauthorized();

            if (profile.IsBlocked)
                return BadRequest("Your account is blocked.");

            ProfileViewModel profileView = AutoMapper.Mapper.Map<UserDTO, ProfileViewModel>(profile);

            var ads = (await uow.AdService.GetAdsByUserId(profile.Id)).ToList();
            ads.RemoveAll(x => x.IsBlocked == true);     

            profileView.Ads = AutoMapper.Mapper.Map<IEnumerable<AdDTO>, List<AdViewModel>>(ads);
            profileView.IsAdmin = User.IsInRole("Admin");
            profileView.IsModerator = User.IsInRole("Moderator");

            return Ok(profileView);
        }

        [HttpGet]
        [Route("allprofiles")]
        public async Task<IHttpActionResult> AllProfiles()
        {
            var profiles = await uow.UserService.GetAllUsers();
            if (profiles == null)
                return this.BadRequest(this.ModelState);
            List<ProfileViewModel> profileViews = AutoMapper.Mapper.Map<List<UserDTO>, List<ProfileViewModel>>(profiles);

            return Ok(profileViews);
        }

        [HttpPut]
        [Route("profile/edit")]
        public async Task<IHttpActionResult> EditProfile([FromBody]EditProfileViewModel newProfile)
        {
            if (User.Identity.GetUserId() == null)
                return this.Unauthorized();

            int userId = User.Identity.GetUserId<int>();
            if (userId != newProfile.Id)
                return BadRequest("It's not your profile!");

            if ((await uow.UserService.GetUserById(userId)).IsBlocked)
                return BadRequest("Your account has been blocked.");

            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            UserDTO profileEdit = AutoMapper.Mapper.Map<EditProfileViewModel, UserDTO>(newProfile);

            await uow.UserService.Update(profileEdit);

            return Ok("Profile has been updated.");
        }

        [HttpPut]
        [Route("profile/password")]
        public async Task<IHttpActionResult> ChangePassword([FromBody]ChangePasswordViewModel changePasswordViewModel)
        {
            if (User.Identity.GetUserId() == null)
                return this.Unauthorized();
            UserDTO user = new UserDTO() { Login = User.Identity.GetUserName(), Password = changePasswordViewModel.OldPassword };

            if (!await uow.UserManagerService.CheckUser(user))
                return BadRequest("Validation failed.");

            if (changePasswordViewModel.ConfirmPassword != changePasswordViewModel.NewPassword)
                ModelState.AddModelError("passwords", "Passwords are not equal");

            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            await uow.UserManagerService.ChangePassword(User.Identity.GetUserId<int>(), changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);

            return Ok("Password has been updated.");
        }

        [HttpDelete]
        [Route("profile/delete")]
        public async Task<IHttpActionResult> DeleteProfile()
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            if (User.Identity.GetUserId() == null)
                return this.Unauthorized();

            await uow.UserService.Delete(User.Identity.GetUserId<int>());

            return Ok("Profile is deleted.");
        }
    }
}
