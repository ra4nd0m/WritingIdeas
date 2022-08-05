using WritingIdeas.Authorization;
using WritingIdeas.Data;
using WritingIdeas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace WritingIdeas.Pages.Ideas
{
    public class AuthBasePageModel:PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        public AuthBasePageModel(
            ApplicationDbContext context,
            IAuthorizationService service,
            UserManager<IdentityUser> userManager
            )
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = service;
        }
    }
}
