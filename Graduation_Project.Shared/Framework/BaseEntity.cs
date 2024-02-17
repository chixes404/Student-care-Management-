using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Framework
{
    public abstract class BaseEntity
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(Order = 110)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        [Required]
        [Column(Order = 120)]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required]
        [Column(Order = 130)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Updated { get; set; }

        [Required]
        [Column(Order = 140)]
        [Display(Name = "Updated By")]
        public Guid UpdatedBy { get; set; }

        //[Timestamp]
        //[Column(Order = 150)]
        //public byte[] RowVersion { get; set; }
    }
}
