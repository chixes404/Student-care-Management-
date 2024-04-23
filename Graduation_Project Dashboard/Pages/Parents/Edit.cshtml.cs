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

namespace Graduation_Project.Dashboard.Pages.Parents
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

        public Parent UserParent { get; set; } = default!;

        [BindProperty]
        public User PaUser { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var parent = await _context.Parents.FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == parent.UserId);
            if (user == null)
            {
                return NotFound();
            }

            UserParent = parent;
            PaUser = user;

            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var NameValidation = _context.Users.Count(x => x.Id != PaUser.Id && x.UserName == PaUser.UserName);
            if (NameValidation != 0)
            {
                ModelState.AddModelError("User.Name", "User name already exists");
                return Page();
            }

            var EmailValidation = _context.Users.Count(x => x.Id != PaUser.Id && x.Email == PaUser.Email);
            if (EmailValidation != 0)
            {
                ModelState.AddModelError("User.Email", "User email already exists");
                return Page();
            }

            var CurrentUserId = _userService.GetCurrentUserID();
            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);

            if (CurrentUser != null)
            {
                PaUser.SchoolId = CurrentUser.SchoolId;
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

                var SecureFileName = Regex.Replace((PaUser.FirstName + PaUser.LastName).ToLower(), "[^0-9A-Za-z_-]", "") + ext;

                var file = Path.Combine(_environment.WebRootPath, "uploads", "users", SecureFileName); // Corrected file path
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                PaUser.ImageURL = "/uploads/users/" + SecureFileName;
            }
            else
            {
                var currentUser = await _context.Users.FindAsync(PaUser.Id);
                if (currentUser != null)
                {
                    PaUser.ImageURL = currentUser.ImageURL;
                }
            }
            var parent = await _context.Parents.FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }
            UserParent = parent;

            try
            {
                //_context.Entry(User).State = EntityState.Detached;
                //_context.Attach(PaUser).State = EntityState.Modified;
                UserParent.IsActive = PaUser.IsActive;
                UserParent.Updated = DateTime.Now;
                UserParent.NationalID = PaUser.NationalId;    
                UserParent.Name = $"{PaUser.FirstName} {PaUser.LastName}";

                //_context.Attach(UserParent).State = EntityState.Modified;
                _context.Entry(parent.User).CurrentValues.SetValues(PaUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(PaUser.Id) && !ParentExists(parent.Id))
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

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
