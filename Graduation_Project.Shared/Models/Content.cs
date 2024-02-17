using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.Framework;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;

//[Table("Content")]
public partial class Content : BaseEntity
{



    [Required]
    [MaxLength(250)]
    [Display(Name = "Page Name")]
    public string PageName { get; set; }

    [Required]
    [Column(TypeName = "ntext")]
    [Display(Name = "Content")]
    [DataType(DataType.MultilineText)]
    public string PageContent { get; set; }









    [ForeignKey("CreatedBy")]
    [Display(Name = "Created By")]
    [InverseProperty("ContentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("UpdatedBy")]
    [Display(Name = "Updated By")]
    [InverseProperty("ContentUpdatedByNavigations")]
    public virtual User UpdatedByNavigation { get; set; }
}
