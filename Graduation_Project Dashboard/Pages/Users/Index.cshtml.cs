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


namespace Graduation_Project_Dashboard.Pages.Users
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

        public IList<User> User { get; set; } = default!;

        public async Task OnGetAsync()
        {
            //User = await _context.Users
            //.ToListAsync();

                User = await _context.Users
                    .FromSqlRaw(@"SELECT u.*
                  FROM dbo.AspNetUsers u
                  INNER JOIN dbo.AspNetUserRoles ur ON u.Id = ur.UserId
                  INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id").ToListAsync();



        }

            public String GetUserRole(Guid id)
            {
                List<Role> roles =  _context.Roles.FromSqlRaw(@"SELECT dbo.AspNetRoles.Id, dbo.AspNetRoles.Name, dbo.AspNetRoles.NormalizedName, dbo.AspNetRoles.ConcurrencyStamp
                    FROM dbo.AspNetRoles INNER JOIN
                     dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId
                    WHERE (dbo.AspNetUserRoles.UserId = {0})", id.ToString()).ToList<Role>();

                string rolesString = "";
                foreach (Role item in roles)
                {
                    if (item != roles.Last())
                    {
                        rolesString += item.Name + "<br/>";
                    }
                    else
                    {
                        rolesString += item.Name;
                    }
                }

                return rolesString;
            }
    }
}
