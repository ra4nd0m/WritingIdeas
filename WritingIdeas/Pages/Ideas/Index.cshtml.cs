using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WritingIdeas.Data;
using WritingIdeas.Models;

namespace WritingIdeas.Pages.Ideas
{
    public class IndexModel : PageModel
    {
        private readonly WritingIdeas.Data.ApplicationDbContext _context;

        public IndexModel(WritingIdeas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Idea> Idea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Idea != null)
            {
                Idea = await _context.Idea.ToListAsync();
            }
        }
    }
}
