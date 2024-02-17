using Graduation_Project.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Reflections.Framework.RoleBasedSecurity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{

    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public partial class UserRole
    {

        public Guid UserId { get; set; }


        public Guid RoleId { get; set; }

        //public virtual User User { get; set; } = null!;
        //public virtual Role Role { get; set; } = null!;
    }
}
