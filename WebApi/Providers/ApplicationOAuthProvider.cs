using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using BLL.Contracts;
using System.Threading.Tasks;
using BLL.DTOs;

namespace WebApi.Providers
{
    public class ApplicationOAuthProvider: OAuthAuthorizationServerProvider
    {
        private IAuthenticationManager authenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        private readonly IUnitOfWorkBLL unitOfWork;

        public ApplicationOAuthProvider(IUnitOfWorkBLL uow)
        {
            unitOfWork = uow;
        }

        //Validates the user object
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        //Checks for the user in DB using the repository and if user is valid initiates “ClaimsIdentity” to generate token.
        // i.e. handling all the token generation
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //GetClaim with BearerTokens
            ClaimsIdentity claim = await unitOfWork.UserManagerService.GetClaims(context.UserName, context.Password);

            UserDTO user = await unitOfWork.UserService.GetUserByLogin(context.UserName);

            if (claim == null || user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            claim.AddClaim(new Claim("UserName", user.Login));

            if (await unitOfWork.UserManagerService.IsUserInRoleAdmin(user.Id))
            {
                claim.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }
            if (await unitOfWork.UserManagerService.IsUserInRoleModerator(user.Id))
            {
                claim.AddClaim(new Claim(ClaimTypes.Role, "Moderator"));
            }


            //cancellation any claims identity associated the the caller
            authenticationManager.SignOut();

            //grant a claims-based identity (token response) to the recipient of the response 
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);  // claim with BearerTokens  
            context.Validated(claim);
        }
    }
}