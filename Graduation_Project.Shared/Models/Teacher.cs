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
    public class Teacher : BaseEntity
    {

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        [ForeignKey("School")]
        public int SchoolId { get; set; }

        public string NationalID { get; set; }


        [ForeignKey("SchoolId")]
        [InverseProperty("Teachers")]
        public virtual School School { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


        public List<File> UploadedFiles { get; set; }


        [ForeignKey("CreatedBy")]
        [Display(Name = "Created By")]
        [InverseProperty("TeacherCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }

        [ForeignKey("UpdatedBy")]
        [Display(Name = "Updated By")]
        [InverseProperty("TeacherUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }



    }
}
