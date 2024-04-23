using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graduation_Project.Shared.Models;

[Table(name: "Users")]
[Index("NormalizedEmail", Name = "EmailIndex")]
public partial class User : IdentityUser<Guid>
{
    

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(14)]
    public string? NationalId { get; set; }

    public string ?Address { get; set; }


    [StringLength(255)]
    public string? ImageURL { get; set; } /*= "/uploads/clinics/2.webp";*/


    [NotMapped]
    public string DisplayText { get { return this.FirstName + " " + this.LastName; } }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }

    [Column("OTP")]
    [StringLength(6)]
    public string? Otp { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Expire { get; set; }


    public int? SchoolId { get; set; }


    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Content> ContentCreatedByNavigations { get; set; } = new List<Content>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Content> ContentUpdatedByNavigations { get; set; } = new List<Content>();

    //[InverseProperty("CreatedByNavigation")]
    //public virtual ICollection<Teacher> TeacherCreatedByNavigations { get; set; } = new List<Teacher>();

    //[InverseProperty("UpdatedByNavigation")]
    //public virtual ICollection<Teacher> TeacherUpdatedByNavigations { get; set; } = new List<Teacher>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    [ForeignKey("SchoolId")]
    public virtual School? School { get; set; }
}