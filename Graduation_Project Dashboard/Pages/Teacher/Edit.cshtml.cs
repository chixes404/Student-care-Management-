using System;
using System.Collections.Generic;
using System.IO; // Add this namespace for Path
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;
using M = Graduation_Project.Shared.Models;
using System.Text.RegularExpressions;
using Graduation_Project_Dashboard.Data;
using Microsoft.AspNetCore.Identity;
using Reflections.Framework.RoleBasedSecurity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Graduation_Project_Dashboard.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;

namespace Graduation_Project.Dashboard.Pages.Teacher
{
    [Authorize(Roles = "Administrator,Configuration")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailService _emailSender;
        private readonly RoleManager<M.Role> _roleManager;
        private readonly IWebHostEnvironment _environment;

        public EditModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context,
            UserResolverService userService,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            IWebHostEnvironment environment,
            ILogger<RegisterModel> logger,
            EmailService emailSender,
            RoleManager<M.Role> roleManager
           )
        {
            _context = context;
            _userService = userService;
            this._environment = environment;
        }

        public M.Teacher UserTeacher { get; set; } = default!;

        [BindProperty]
        public User User { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedSubjects { get; set; }

        [BindProperty]
        public List<int> SelectedGradeIds { get; set; }

        public SelectList SubjectsList { get; set; }
        public SelectList GradesList { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == teacher.UserId);
            if (user == null)
            {
                return NotFound();
            }

            UserTeacher = teacher;
            User = user;
            if (id == null)
            {
                return NotFound();
            }

          

            // Populate SubjectsList and GradesList
            SubjectsList = new SelectList(await _context.Subjects.ToListAsync(), "Id", "SubjectName");
            GradesList = new SelectList(await _context.Grades.ToListAsync(), "Id", "GradeTitle");

            // Populate SelectedSubjects and SelectedGradeIds based on the user's current associations
            SelectedSubjects = await _context.TeacherSubjects
                .Where(ts => ts.TeacherId == teacher.Id)
                .Select(ts => ts.SubjectId)
                .ToListAsync();

            SelectedGradeIds = await _context.TeacherGrades
                .Where(tg => tg.TeacherId == teacher.Id)
                .Select(tg => tg.GradeId)
                .ToListAsync();


            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var NameValidation = _context.Users.Count(x => x.Id != User.Id && x.UserName == User.UserName);
            if (NameValidation != 0)
            {
                ModelState.AddModelError("User.Name", "User name already exists");
                return Page();
            }

            var EmailValidation = _context.Users.Count(x => x.Id != User.Id && x.Email == User.Email);
            if (EmailValidation != 0)
            {
                ModelState.AddModelError("User.Email", "User email already exists");
                return Page();
            }

            var CurrentUserId = _userService.GetCurrentUserID();
            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);

            if (CurrentUser != null)
            {
                User.SchoolId = CurrentUser.SchoolId;
            }
            else
            {
                ModelState.AddModelError("", "Current user not found.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload != null)
            {
                string[] permittedExtensions = { ".png", ".jpg" }; // Corrected file extensions

                var ext = Path.GetExtension(Upload.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("User.ImageUrl", "The file extension is invalid");
                    return Page();
                }

                var SecureFileName = Regex.Replace((User.FirstName + User.LastName).ToLower(), "[^0-9A-Za-z_-]", "") + ext;

                var file = Path.Combine(_environment.WebRootPath, "uploads", "Teachers", SecureFileName); // Corrected file path
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                User.ImageURL = "/uploads/Teachers/" + SecureFileName;
            }
            else
            {
                var currentUser = await _context.Users.FindAsync(User.Id);
                if (currentUser != null)
                {
                    User.ImageURL = currentUser.ImageURL;
                }
            }
            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            UserTeacher = teacher;

            try
            {
                //_context.Entry(User).State = EntityState.Detached;
                //_context.Attach(User).State = EntityState.Modified;
                UserTeacher.IsActive = User.IsActive;
                UserTeacher.UserId= User.Id;
                UserTeacher.Updated = DateTime.Now;
                UserTeacher.NationalID = User.NationalId;
                UserTeacher.Name = $"{User.FirstName} {User.LastName}";

                _context.Attach(UserTeacher).State = EntityState.Modified;

                teacher.User.IsActive = User.IsActive;
                teacher.User.NationalId = User.NationalId;
                teacher.User.Address = User.Address;
               
                teacher.User.FirstName = User.FirstName;
                teacher.User.LastName = User.LastName;



                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id) && !TeacherExists(teacher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await UpdateTeacherSubjects(teacher.Id, SelectedSubjects);
            await UpdateTeacherGrades(teacher.Id, SelectedGradeIds);
            return RedirectToPage("./Index");
        }
        private async Task UpdateTeacherSubjects(int teacherId, List<int> selectedSubjects)
        {
            // Remove existing associations
            var existingSubjects = await _context.TeacherSubjects.Where(ts => ts.TeacherId == teacherId).ToListAsync();
            _context.TeacherSubjects.RemoveRange(existingSubjects);

            // Add new associations
            foreach (var subjectId in selectedSubjects)
            {
                _context.TeacherSubjects.Add(new TeacherSubject { TeacherId = teacherId, SubjectId = subjectId });
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateTeacherGrades(int teacherId, List<int> selectedGradeIds)
        {
            // Remove existing associations
            var existingGrades = await _context.TeacherGrades.Where(tg => tg.TeacherId == teacherId).ToListAsync();
            _context.TeacherGrades.RemoveRange(existingGrades);

            // Add new associations
            foreach (var gradeId in selectedGradeIds)
            {
                _context.TeacherGrades.Add(new TeacherGrade { TeacherId = teacherId, GradeId = gradeId });
            }

            await _context.SaveChangesAsync();
        }
        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
