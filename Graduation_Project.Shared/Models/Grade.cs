using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class Grade : BaseEntity
    {

       public string GradeTitle { get; set; }

        [JsonIgnore]
        public ICollection<Student> Students { get; set; }
        public int NumberOfStudents   // iinstead of Student Count Classes
        {
            get { return Students.Count; } // Return the private field
        }
        public ICollection<TeacherGrade> TeacherGrades { get; set; }


    }
}
