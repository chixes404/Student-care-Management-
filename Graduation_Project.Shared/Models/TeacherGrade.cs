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
    public class TeacherGrade : BaseEntity
    {

        public int TeacherId { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
        public Teacher Teacher { get; set; }

    }
}