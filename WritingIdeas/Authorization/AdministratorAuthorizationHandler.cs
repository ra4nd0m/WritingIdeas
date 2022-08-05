using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using WritingIdeas.Models;
namespace WritingIdeas.Authorization
{
    public class AdministratorAuthorizationHandler
        :AuthorizationHandler<OperationAuthorizationRequirement, Idea>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Idea resource)
        {
            if (context.User == null)
                return Task.CompletedTask;
            if (context.User.IsInRole(Constants.AdministratorRole))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
