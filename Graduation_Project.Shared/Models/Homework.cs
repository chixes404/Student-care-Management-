using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public  class Homework : BaseEntity
    {
        public int TeacherId { get; set; }  
        public int? GradeId { get; set; }
        public int? ClassId { get; set; }
        public string? FilePath { get; set; }

         public int? SubjectId { get; set; }
        public string ? Message { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Subject Subject { get; set; }

    }
}
