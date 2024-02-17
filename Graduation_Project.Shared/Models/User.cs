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
    public string Address { get; set; }


    [StringLength(255)]
    public string? ImageURL { get; set; } /*= "/uploads/clinics/2.webp";*/


    [NotMapped]
    public string DisplayText { get { return this.FirstName + " " + this.LastName; } }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }

    

   


    
    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Content> ContentCreatedByNavigations { get; set; } = new List<Content>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Content> ContentUpdatedByNavigations { get; set; } = new List<Content>();

    
    //[InverseProperty("CreatedByNavigation")]
    //public virtual ICollection<Gender> GenderCreatedByNavigations { get; set; } = new List<Gender>();

    //[InverseProperty("UpdatedByNavigation")]
    //public virtual ICollection<Gender> GenderUpdatedByNavigations { get; set; } = new List<Gender>();

  
    //[InverseProperty("CreatedByNavigation")]
    //public virtual ICollection<Profile> ProfileCreatedByNavigations { get; set; } = new List<Profile>();

    //[InverseProperty("UpdatedByNavigation")]
    //public virtual ICollection<Profile> ProfileUpdatedByNavigations { get; set; } = new List<Profile>();

    //[InverseProperty("User")]
    //public virtual Profile ProfileUser { get; set; }

 

    [InverseProperty("User")]
    public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();


    [InverseProperty("User")]
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    [InverseProperty("User")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

   
    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    //public virtual ICollection<UserRole> UserRoles { get; set; }
}