using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WritingIdeas.Data;
using WritingIdeas.Models;
using WritingIdeas.Authorization;

namespace WritingIdeas.Pages.Ideas
{
    public class CreateModel : AuthBasePageModel
    {
        public CreateModel(ApplicationDbContext context,
            IAuthorizationService service,
            UserManager<IdentityUser> userManager
            ):base(context,service,userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Idea Idea { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (UserManager.GetUserId(User) == null)
                return Challenge();
            Idea.OwnerID = UserManager.GetUserId(User);
            if (!ModelState.IsValid || Context.Idea == null || Idea == null)
            {
                return Page();
            }
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Idea, IdeaOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Challenge();
            }
            Context.Idea.Add(Idea);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
