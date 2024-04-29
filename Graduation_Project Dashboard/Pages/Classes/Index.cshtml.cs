using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Graduation_Project_Dashboard.Data;



namespace Graduation_Project_Dashboard.Pages.Classes
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

        public IList<Class> Classes { get; set; } = default!;

        public async Task OnGetAsync()
        {

            Classes = await _context.Classes
                    .Include(c => c.Students) // Eager loading of the Students collection
                    .ToListAsync();
        }





    }
}
