using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public int NumberOfProducts    // you must turn it into Method not property , because this is Driven attribute
                                              //which will executed in runtime only , but when you make it as property it will turn into column in db 
        {
            get { return Products.Count; } // Return the private field
        }

        [StringLength(255)]
        public string? ImageURL { get; set; } /*= "/uploads/clinics/2.webp";*/

        public bool IsActive { get; set; }


    }
}
