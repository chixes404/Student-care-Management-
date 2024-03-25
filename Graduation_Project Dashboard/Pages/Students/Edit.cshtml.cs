using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Reflections.Framework.RoleBasedSecurity;
using M = Graduation_Project.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Graduation_Project_Dashboard.Pages.Students
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<M.Role> _roleManager;
        private readonly IWebHostEnvironment _environment;

        public EditModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context,
            UserResolverService userService,
            UserManager<User> userManager,
            RoleManager<M.Role> roleManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            this._environment = environment;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;


        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public int SelectedGradeId { get; set; } // Added property for selected grade

        public SelectList Grades { get; set; } // Added property for dropdown list

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var student  = await _context.Students
                .Include(s => s.Grade) // Include the Grade navigation property
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            Student = student;

            Grades = new SelectList(await _context.Grades.ToListAsync(), "Id", "GradeTitle");
            SelectedGradeId = Student.GradeId;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
       
            /***************************************************************/
            if (Upload != null)
            {

                string[] permittedExtensions = { ".png", "jpg" };

                var ext = Path.GetExtension(Upload.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    // The extension is invalid ... discontinue processing the file
                    ModelState.AddModelError("User.ImageUrl", "The file extension is invalid");
                    return Page();
                }


                var SecureFileName = Regex.Replace((Student.Name).ToLower(), "[^0-9A-Za-z_-]", "") + ext;

                var file = Path.Combine(_environment.WebRootPath, "uploads\\students", SecureFileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                Student.ImageURL = "/uploads/users/" + SecureFileName;
            }


            /***************************************************************/

            Student.GradeId = SelectedGradeId;
            _context.Attach(Student).State = EntityState.Modified;







            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(Student.Id))
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

        private bool UserExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
