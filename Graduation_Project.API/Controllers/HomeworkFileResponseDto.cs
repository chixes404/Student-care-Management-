using Graduation_Project.API.Data;
using Graduation_Project.Shared.Models;

namespace Graduation_Project.API.Controllers
{
        public class HomeworkFileResponseDto
        {
        private readonly ApplicationDatabaseContext _context;

        public HomeworkFileResponseDto(ApplicationDatabaseContext context)
        {
            _context = context;

        }
        public string TeacherName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadedDate { get; set; }
        public string SubjectName { get; set; }
      
    }
}