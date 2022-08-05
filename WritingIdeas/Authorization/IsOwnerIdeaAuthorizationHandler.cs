using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using WritingIdeas.Models;
namespace WritingIdeas.Authorization
{
    public class IsOwnerIdeaAuthorizationHandler
                  : AuthorizationHandler<OperationAuthorizationRequirement, Idea>
    {
        UserManager<IdentityUser> _userManager;
        public IsOwnerIdeaAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Idea resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }
            if(requirement.Name!=Constants.CreateOperationName&&requirement.Name!=Constants.DeleteOperationName&&
                requirement.Name != Constants.UpdateOperationName && requirement.Name != Constants.ReadOperationName)
            {
                return Task.CompletedTask;
            }
            if (_userManager.GetUserId(context.User) == resource.OwnerID)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

    }
}
