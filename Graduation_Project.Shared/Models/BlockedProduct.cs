using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class BlockedProduct  :BaseEntity
    {
        public int ProductId { get; set; }
        public int StudentId { get; set; }

        public bool IsBlocked { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
  
        [ForeignKey("StudentId")]

        public Student Student { get; set; }



    }
}
