using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Reflections.Framework.RoleBasedSecurity;

namespace Graduation_Project.Shared.Models;
[Table(name: "RoleClaims")]
[Index("RoleId", Name = "IX_RoleClaims_RoleId")]
public partial class RoleClaim
{
    [Key]
    public int Id { get; set; }

    public Guid RoleId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("RoleClaims")]
    public virtual Role Role { get; set; }
}
