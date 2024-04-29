using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class ProductDtoss
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public bool IsBlocked { get; set; }
    }

}
