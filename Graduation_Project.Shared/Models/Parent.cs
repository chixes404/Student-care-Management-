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
    public partial class Parent 
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }


        [Required]
        public int ? SchoolId { get; set; }

        //public string NationalID {  get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [Column(Order = 110)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }


        [Required]
        [Column(Order = 130)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Updated { get; set; }


        //[ForeignKey("CreatedBy")]
        //[Display(Name = "Created By")]
        //[InverseProperty("ParentCreatedByNavigations")]
        //public virtual User ?CreatedByNavigation { get; set; }

        //[ForeignKey("UpdatedBy")]
        //[Display(Name = "Updated By")]
        //[InverseProperty("ParentUpdatedByNavigations")]
        //public virtual User ?UpdatedByNavigation { get; set; }
    }
}
