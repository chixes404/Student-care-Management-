using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class TeacherClass
    {
        public int TeacherId { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
        public Teacher Teacher { get; set; }
    }
}
