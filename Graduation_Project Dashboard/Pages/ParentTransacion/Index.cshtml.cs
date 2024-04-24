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
using Graduation_Project_Dashboard.Data;
using M = Graduation_Project.Shared.Models;


namespace Graduation_Project_Dashboard.Pages.ParentTransacion
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;

        public IndexModel(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public IList<M.ParentTransaction> ParentTransactions { get; set; } = default!;

        public async Task OnGetAsync()
        {

            ParentTransactions = await _context.ParentTransactions
                .Include(pt => pt.Student)
                .Include(pt => pt.Parent)
                .ToListAsync();
        }





    }
}
