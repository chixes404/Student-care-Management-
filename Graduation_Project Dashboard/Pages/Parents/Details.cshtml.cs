//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using Graduation_Project_Dashboard.Services;
//using Graduation_Project.Shared.Models;
//using Graduation_Project_Dashboard.Data;
//using Microsoft.AspNetCore.Authorization;
//using static Graduation_Project_Dashboard.Pages.Parents.IndexModel;

//namespace Graduation_Project_Dashboard.Pages.Parents
//{
//    [Authorize(Roles = "Administrator")]
//    public class DetailsModel : PageModel
//    {
//        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;

//        public DetailsModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context)
//        {
//            _context = context;
//        }
//        public IList<UserProfileViewModel> ParentUser { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            ParentUser = await _context.Users
//                   .Where(user => user.Id == id) // Filter by user Id
//                   .Join(
//                       _context.Parents,
//                       user => user.Id,
//                       parent => parent.UserId,
//                       (user, parent) => new
//                       {
//                           User = user,
//                           Parent = parent
//                       }
//                   )
//                   .GroupJoin(
//                       _context.Students,
//                       parentUser => parentUser.Parent.Id,
//                       student => student.ParentId,
//                       (parentUser, student) => new UserProfileViewModel
//                       {
//                           ParentId = parentUser.Parent.Id,
//                           FirstName = parentUser.User.FirstName + " " + parentUser.User.LastName,
//                           Email = parentUser.User.Email,
//                           MobileNumber = parentUser.User.PhoneNumber,
//                           NationalId = parentUser.User.NationalId,
//                           Address = parentUser.User.Address,
//                           UserId = parentUser.Parent.UserId,
//                           StudentList = string.Join(", ", student.Select(s => s.Name).ToList()), // Corrected Select usage                           ImageURL = parentUser.User.ImageURL,
//                           SchoolName = parentUser.User.School.Name
//                       }
//                   )
//                   .ToListAsync(); return Page();
//        }
//        public class UserProfileViewModel
//        {
//            public string FirstName { get; set; }
//            public string LastName { get; set; }
//            public string Email { get; set; }
//            public string MobileNumber { get; set; }
//            public string NationalId { get; set; }
//            public string Address { get; set; }
//            public int ParentId { get; set; }
//            public Guid UserId { get; set; }
//            public string ? StudentList { get; set; }

//            public string ImageURL { get; set; }
//            public bool IsActive { get; set; }
//            public string? SchoolName { get; set; }
//        }

//    }
//}
