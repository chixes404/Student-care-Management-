using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;
//[Table(name: "UserClaims")]
[Index("UserId", Name = "IX_UserClaims_UserId")]
public partial class UserClaim
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserClaims")]
    public virtual User User { get; set; }
}
