//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Graduation_Project_Dashboard.Code;
//using Graduation_Project_Dashboard.Data;
//using Graduation_Project_Dashboard.Services;
//using Graduation_Project.Shared.Models;
//using Microsoft.AspNetCore.Authorization;

//namespace Graduation_Project.WebUI.Pages.Pages
//{
//    [Authorize(Roles = "Administrator,Content")]
//    public class EditModel : PageModel
//    {
//        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
//        private readonly UserResolverService _userService;

//        public EditModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context, UserResolverService userService)
//        {
//            _context = context;
//            _userService = userService;
//        }

//        [BindProperty]
//        public Content Content { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var content =  await _context.Contents
                
//                .Include(c => c.CreatedByNavigation)
//                .Include(c => c.UpdatedByNavigation)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (content == null)
//            {
//                return NotFound();
//            }
//            Content = content;
//           ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
//           ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see https://aka.ms/RazorPagesCRUD.
//        public async Task<IActionResult> OnPostAsync()
//        {
//            var NameValidation = _context.Contents.Count(x => x.Id != Content.Id && x.PageName == Content.PageName);
//            if (NameValidation != 0)
//            {
//                ModelState.AddModelError("Content.PageName", "Content page name already exists");
//                return Page();
//            }

//            var User = _context.Users.FirstOrDefault(u => u.Id == _userService.GetCurrentUserID());

//            Content.Updated = DateTime.Now;
//            Content.UpdatedBy = User.Id;
//            Content.UpdatedByNavigation = User;



//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            _context.Attach(Content).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ContentExists(Content.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return RedirectToPage("./Index");
//        }

//        private bool ContentExists(int id)
//        {
//            return _context.Contents.Any(e => e.Id == id);
//        }
//    }
//}
