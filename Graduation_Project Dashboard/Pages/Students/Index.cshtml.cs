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

namespace Graduation_Project_Dashboard.Pages.Students
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

        public IList<StudentInfoModel> StudentInfo { get; set; } = new List<StudentInfoModel>();

        public async Task OnGetAsync()
        {
            // Join Student, Parent, and User tables to get the required information
            var studentInfoQuery = from student in _context.Students
                                   join parent in _context.Parents on student.ParentId equals parent.Id
                                   join user in _context.Users on parent.UserId equals user.Id
                                   select new StudentInfoModel
                                   {
                                       Id = student.Id,
                                       StudentName = student.Name,
                                       StudentDOB = student.DateOfBirth,
                                       StudentGrade = student.Grade.GradeTitle,
                                       StudentAge = student.age,
                                       ParentEmail = user.Email,
                                       ParentMobileNo = user.PhoneNumber,
                                       Active = student.Active,
                                       QRCodeUrl = student.QrCodeUrl
                                   };

            // Execute the query and store the results in the StudentInfo property
            StudentInfo = await studentInfoQuery.ToListAsync();
        }

        // Helper method to calculate age based on date of birth
   
    }

    // Model to store the required information
    public class StudentInfoModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public DateOnly StudentDOB { get; set; }
        public string StudentGrade { get; set; }
        public int? StudentAge { get; set; }
        public string ParentEmail { get; set; }

        public string ParentMobileNo { get; set; }

        public bool Active { get; set; }
        public string ? QRCodeUrl {  get; set; }
    }
}
