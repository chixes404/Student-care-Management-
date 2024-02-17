using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Data;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;

namespace Graduation_Project_Dashboard.Pages.Pages
{
    [Authorize(Roles = "Administrator,Content")]
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;

        public IndexModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public IList<Content> Content { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Content = await _context.Contents
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.UpdatedByNavigation)
            .ToListAsync();
        }
    }
}
