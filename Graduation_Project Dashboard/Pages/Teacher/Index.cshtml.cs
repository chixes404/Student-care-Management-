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


namespace Graduation_Project_Dashboard.Pages.Teacher
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

        public IList<TeacherViewModel> Teachers { get; set; } = default!;

        public async Task OnGetAsync()
        {
            //User = await _context.Users
            //.ToListAsync();

            Teachers = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.TeacherSubjects)
                .Include(t => t.TeacherGrades)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    SchoolName = t.User.School.Name, // Assuming SchoolName is a property of User
                    ImageURL = t.User.ImageURL,
                    IsActive = t.IsActive,
                    NationalId = t.NationalID,
                    Email = t.User.Email,
                    Address = t.User.Address,
                    MobileNumber = t.User.PhoneNumber,
                    Subjects = t.TeacherSubjects.Select(ts => ts.Subject).ToList(),
                    Grades = t.TeacherGrades.Select(tg => tg.Grade).ToList()
                })
                .ToListAsync();
        }

        //public String GetUserRole(Guid? id)
        //{
        //    List<Role> roles = _context.Roles.FromSqlRaw(@"SELECT dbo.AspNetRoles.Id, dbo.AspNetRoles.Name, dbo.AspNetRoles.NormalizedName, dbo.AspNetRoles.ConcurrencyStamp
        //            FROM dbo.AspNetRoles INNER JOIN
        //             dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId
        //            WHERE (dbo.AspNetUserRoles.UserId = {0})", id.ToString()).ToList<Role>();

        //    string rolesString = "";
        //    foreach (Role item in roles)
        //    {
        //        if (item != roles.Last())
        //        {
        //            rolesString += item.Name + "<br/>";
        //        }
        //        else
        //        {
        //            rolesString += item.Name;
        //        }
        //    }

        //    return rolesString;
        //}


        public class TeacherViewModel
        {
            public int Id { get; set; }
            public Guid? UserId { get; set; }
            public string Name { get; set; }
            public string SchoolName { get; set; }
            public string ImageURL { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
            public string NationalId { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string MobileNumber { get; set; }
            public List<Subject> Subjects { get; set; }
            public List<Grade> Grades { get; set; }
        }

    }
}
