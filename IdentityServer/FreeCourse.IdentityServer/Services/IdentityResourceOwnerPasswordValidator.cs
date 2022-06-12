using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existsUser = await _userManager.FindByEmailAsync(context.UserName);
            if (existsUser == null)
            {
                AddCustomErrorToResponse(context);
                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(existsUser, context.Password);
            if (!passwordCheck)
            {
                AddCustomErrorToResponse(context);
                return;
            }

            context.Result = new GrantValidationResult(existsUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }

        private static void AddCustomErrorToResponse(ResourceOwnerPasswordValidationContext context)
        {
            var errors = new Dictionary<string, object>();
            errors.Add("errors", new List<string>
                {
                    "Email veya şifreniz yanlış."
                });

            context.Result.CustomResponse = errors;
        }
    }
}