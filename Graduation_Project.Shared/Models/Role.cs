using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;
[Table(name: "Roles")]
public partial class Role : IdentityRole<Guid>
{
    //public string Description { get; set; }


    [InverseProperty("Role")]
    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    //public virtual ICollection<UserRole> UserRoles { get; set; }
}
