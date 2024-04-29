using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class TeacherInfoDto
    {
        public int Id { get; set; }
        public Guid ?UserId { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public int ?SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }

        public string SubjectTitle { get; set; }



    }
}
