using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Graduation_Project.Shared.Models
{
    public partial class Student : BaseEntity
    {


        public string Name { get; set; } = string.Empty;
         
        public DateOnly DateOfBirth { get; set; }

        public int GradeId { get; set; }

        [StringLength(255)]
        public string? ImageURL { get; set; } /*= "/uploads/clinics/2.webp";*/

        public bool Active { get; set; } = true; // Add a property to indicate active status, 


        [Required]
        [ForeignKey("Parent")]
        public int ParentId { get; set; }

        [Required]
        [ForeignKey("School")]
        public int SchoolId { get; set; }






        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }

        [ForeignKey("SchoolId")]
        [InverseProperty("Students")]
        public virtual School School { get; set; }

        public List<BlockedProduct> BlockedProducts { get; set; }
        public List<CanteenTransaction> canteenTransaction { get; set; }
        public virtual Wallet wallet { get; set; }

        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }
    }
}
