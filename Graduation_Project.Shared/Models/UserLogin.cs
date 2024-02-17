using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;
[Table(name: "UserLogins")]
[PrimaryKey("LoginProvider", "ProviderKey")]
[Index("UserId", Name = "IX_UserLogins_UserId")]
public partial class UserLogin
{
    [Key]
    public string LoginProvider { get; set; }

    [Key]
    public string ProviderKey { get; set; }

    public string ProviderDisplayName { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserLogins")]
    public virtual User User { get; set; }
}
