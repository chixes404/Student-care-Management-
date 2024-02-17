using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Data;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace Graduation_Project_Dashboard.Pages.Pages
{
    [Authorize(Roles = "Administrator,Content")]
    public class DeleteModel : PageModel
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;

        public DeleteModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Content Content { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.UpdatedByNavigation).FirstOrDefaultAsync(m => m.Id == id);

            if (content == null)
            {
                return NotFound();
            }
            else
            {
                Content = content;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                Content = content;
                _context.Contents.Remove(Content);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
