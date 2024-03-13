using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class Grade : BaseEntity
    {

       public string GradeTitle { get; set; }


        public ICollection<Student> Students { get; set; }

    }
}
