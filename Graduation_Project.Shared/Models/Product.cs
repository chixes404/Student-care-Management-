    using Graduation_Project.Shared.Framework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using System.Linq;
    using System.Text;
using System.Threading.Tasks;

    namespace Graduation_Project.Shared.Models
    {
        public class Product : BaseEntity
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public int CategoryId { get; set; }

            public bool IsActive { get; set; }

            [StringLength(255)]
            public string? ImageURL { get; set; } /*= "/uploads/clinics/2.webp";*/

            public int Amount { get; set; }

            public decimal Price { get; set; }

            [ForeignKey("CategoryId")]
            [InverseProperty("Products")]
             [JsonIgnore]

        public virtual Category category { get; set; }
        public ICollection<CanteenTransactionProduct> CanteenTransactionProducts { get; set; }


    }
}
