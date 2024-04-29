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
    public class CanteenTransactionProduct 
    {
        [Key]
        [ForeignKey("CanteenTransaction")]
        public int CanteenTransactionId { get; set; }

        [Key]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        // Navigation properties
        public virtual CanteenTransaction CanteenTransaction { get; set; }

        public virtual Product Product { get; set; }
    }
}
