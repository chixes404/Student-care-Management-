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

namespace Graduation_Project_Dashboard.Pages.Users
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
        public User User { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public IFormFile ? Upload { get; set; }
        public class InputModel
        {
            [Required, MinLength(1, ErrorMessage = "At least one item required")]
            [Display(Name = "Roles")]
            public List<string> RoleId { get; set; }         
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            //var Appuser = await _userManager.FindByEmailAsync(user.Email);

            //var roles = await _userManager.GetRolesAsync(Appuser);

          


            if (user == null)
            {
                return NotFound();
            }
            User = user;
            var userRoles = await _userManager.GetRolesAsync(User);

            // Fetch role IDs for the user's roles
            var roleIds = userRoles.Select(role => _roleManager.Roles.FirstOrDefault(r => r.Name == role)?.Id);

            // Remove nulls and convert to List<string>
            Input = new InputModel
            {
                RoleId = roleIds.Where(id => id != null).Select(id => id.ToString()).ToList()
            };

            //       var usersWithRoles = await _context.Users
            //.FromSqlRaw(@"SELECT *
            //             FROM dbo.AspNetUsers
            //             WHERE (Id IN
            //              (SELECT UserId
            //              FROM dbo.AspNetUserRoles
            //              WHERE (RoleId IN
            //              (SELECT Id
            //              FROM dbo.AspNetRoles
            //              WHERE (Name <> N'user')))))")
            //.ToListAsync();

            //ViewData["Roles"] = new SelectList(usersWithRoles.SelectMany(u => u.Roles).ToList(), "Id", "Name");


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
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

            User.SchoolId = CurrentUser.SchoolId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            /***************************************************************/
            if (Upload != null)
            {

                string[] permittedExtensions = { ".png" , "jpg" };

                var ext = Path.GetExtension(Upload.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    // The extension is invalid ... discontinue processing the file
                    ModelState.AddModelError("User.ImageUrl", "The file extension is invalid");
                    return Page();
                }


                var SecureFileName = Regex.Replace((User.FirstName + User.LastName).ToLower(), "[^0-9A-Za-z_-]", "") + ext;

                var file = Path.Combine(_environment.WebRootPath, "uploads\\users", SecureFileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                User.ImageURL = "/uploads/users/" + SecureFileName;
            }

            /***************************************************************/

            _context.Attach(User).State = EntityState.Modified;

            var Appuser = await _userManager.FindByEmailAsync(User.Email);

            var roles = await _userManager.GetRolesAsync(Appuser);
            await _userManager.RemoveFromRolesAsync(Appuser, roles.ToArray());



            foreach (string item in Input.RoleId)
            {
                var defaultrole = _roleManager.FindByIdAsync(item).Result;
                if (defaultrole != null)
                {
                    IdentityResult roleresult = await _userManager.AddToRoleAsync(Appuser, defaultrole.Name);
                }
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
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

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
