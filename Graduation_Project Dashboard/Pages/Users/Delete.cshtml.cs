using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Data;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project_Dashboard.Code;

namespace Graduation_Project_Dashboard.Pages.Users
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;

        public DeleteModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                User = user;
                DocumentSetting.DeleteFile(User.ImageURL);
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
