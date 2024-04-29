using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class BlockedProductDto
    {
        public int ProductId { get; set; }
        public int StudentId { get; set; }
        public bool IsBlocked { get; set; }
    }

}
