using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflections.Framework.RoleBasedSecurity;

namespace Graduation_Project.Shared.Models
{
    public partial class Parent : BaseEntity
    {


        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        [ForeignKey("School")]
        public int SchoolId { get; set; }

        public string NationalID {  get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


        [ForeignKey("CreatedBy")]
        [Display(Name = "Created By")]
        [InverseProperty("ParentCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }

        [ForeignKey("UpdatedBy")]
        [Display(Name = "Updated By")]
        [InverseProperty("ParentUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
