using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Data;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;

namespace Graduation_Project_Dashboard.Pages.Pages
{
    [Authorize(Roles = "Administrator,Content")]
    public class CreateModel : BasePageModel<CreateModel>
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;

        public CreateModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context, UserResolverService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Content Content { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult>
            OnPostAsync()
        {


            var User = _context.Users.FirstOrDefault(u => u.Id == _userService.GetCurrentUserID());

            Content.Created = DateTime.Now;
            Content.CreatedBy = User.Id;
            Content.Updated = DateTime.Now;
            Content.UpdatedBy = User.Id;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Contents.Add(Content);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
