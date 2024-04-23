using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Graduation_Project.Dashboard.Pages.Parents
{


    [Authorize(Roles = "Administrator,Configuration")]
    public class DeleteModel : PageModel
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;

        public DeleteModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context )
        {
            _context = context;
        }

        [BindProperty]
        public Parent Parent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var parent =  await _context.Parents.FirstOrDefaultAsync(x=>x.Id==id);
            if (parent == null)
            {
                return NotFound();
            }
            else
            {
                Parent = parent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent != null)
            {
                parent.IsActive = false;
                parent.Updated = DateTime.UtcNow;
                // Find associated user and make them inactive
                var user = await _context.Users.FindAsync(parent.UserId);
                if (user != null)
                {
                    user.IsActive = false;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
