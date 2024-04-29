using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class Class : BaseEntity
    {

        public string ClassTitle { get; set; }


        public ICollection<Student> Students { get; set; } = new List<Student>();
        public int NumberOfStudents   // iinstead of Student Count Classes
        {
            get { return Students.Count; } // Return the private field
        }

        public ICollection<TeacherClass> TeacherClasses { get; set; }

    }
}
