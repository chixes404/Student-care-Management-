using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class TeacherSubject : BaseEntity
    {



        public int TeacherId { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }

    }
}