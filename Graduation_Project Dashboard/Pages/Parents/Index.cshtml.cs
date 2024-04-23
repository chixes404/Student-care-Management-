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

namespace Graduation_Project_Dashboard.Pages.Parents
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

        public IList<UserProfileViewModel> ParentUser { get; set; } = default!;

        public async Task OnGetAsync()
        {
            //User = await _context.Users
            //.ToListAsync();

            ParentUser = await _context.Users
               .Join(
                   _context.Parents,
                   user => user.Id,
                   parent => parent.UserId,
                   (user, parent) => new UserProfileViewModel
                   {
                       ParentId = parent.Id,
                       Name = user.FirstName + " " + user.LastName,
                       Email = user.Email,
                       MobileNumber = user.PhoneNumber,
                       NationalId = user.NationalId,
                       Address = user.Address,
                       UserId = parent.UserId,
                       StudentList = parent.Students.Count,
                       IsActive = parent.IsActive,
                       ImageURL = user.ImageURL,
                       SchoolName = user.School.Name
                   }
               )
               .ToListAsync();
           

        }

        public String GetUserRole(Guid ?id)
        {
            List<Role> roles = _context.Roles.FromSqlRaw(@"SELECT dbo.AspNetRoles.Id, dbo.AspNetRoles.Name, dbo.AspNetRoles.NormalizedName, dbo.AspNetRoles.ConcurrencyStamp
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


        public class UserProfileViewModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string MobileNumber { get; set; }
            public string NationalId { get; set; }
            public string Address { get; set; }
            public int  ParentId { get; set; }
            public Guid? UserId { get; set; }
            public int StudentList { get; set; }

            public string ImageURL { get; set; }
            public bool IsActive { get; set; }
            public string ? SchoolName {  get; set; }
        }

    }
}
