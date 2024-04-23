using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using M = Graduation_Project.Shared.Models;


namespace Graduation_Project.Dashboard.Pages.Teacher
{


    [Authorize(Roles = "Administrator,Configuration")]
    public class DeleteModel : PageModel
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;

        public DeleteModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public M.Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            else
            {
                Teacher = teacher;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                teacher.IsActive = false;
                teacher.Updated = DateTime.UtcNow;
                // Find associated user and make them inactive
                var user = await _context.Users.FindAsync(teacher.UserId);
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
