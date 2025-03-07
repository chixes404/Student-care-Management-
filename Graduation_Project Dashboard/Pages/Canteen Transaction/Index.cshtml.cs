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


namespace Graduation_Project_Dashboard.Pages.CanteenTransaction
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;

        public IndexModel(ApplicationDatabaseContext context ,  UserResolverService userService)
        {
            _context = context;
            _userService = userService; 
        }

        public IList<M.CanteenTransaction> CanteenTransactions { get; set; } = default!;

        public async Task OnGetAsync()
        {

            CanteenTransactions = await _context.CanteenTransactions
                .Include(pt => pt.Student)
                .ToListAsync();
        }





    }
}
