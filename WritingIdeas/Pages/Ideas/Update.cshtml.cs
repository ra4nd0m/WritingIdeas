using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WritingIdeas.Data;
using WritingIdeas.Models;

namespace WritingIdeas.Pages.Ideas
{
    public class UpdateModel : PageModel
    {
        private readonly WritingIdeas.Data.ApplicationDbContext _context;

        public UpdateModel(WritingIdeas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Idea Idea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Idea == null)
            {
                return NotFound();
            }

            var idea =  await _context.Idea.FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }
            Idea = idea;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Idea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaExists(Idea.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IdeaExists(int id)
        {
          return (_context.Idea?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
