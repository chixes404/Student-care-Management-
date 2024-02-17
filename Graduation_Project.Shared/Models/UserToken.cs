using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;
//[Table(name: "UserTokens")]
[PrimaryKey("UserId", "LoginProvider", "Name")]
public partial class UserToken
{
    [Key]
    public Guid UserId { get; set; }

    [Key]
    public string LoginProvider { get; set; }

    [Key]
    public string Name { get; set; }

    public string Value { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserTokens")]
    public virtual User User { get; set; }
}
