using Reflections.Framework.RoleBasedSecurity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Shared.Framework;

namespace Graduation_Project.Shared.Models
{
    public partial class Chat : BaseEntity
    {

        public Chat(Guid userId)
        {
            CreatedBy = userId;
            UpdatedBy = userId;
            this.UserId = userId;
        }

        public Guid UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public int ?SchoolId { get; set; }

        [ForeignKey("UserId")]
        //[InverseProperty("ChatUsers")]
        public virtual User? user { get; set; }
    }
}



