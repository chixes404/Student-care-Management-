using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class ParentWithoutStudentDto
    {


        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid ?UserId { get; set; }

        public int ParentId { get; set; }

        public int ?SchoolId { get; set; }

        public string SchoolName {  get; set; }
        public string ImageUrl { get; set; }
    }
}
