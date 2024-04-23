using Reflections.Framework.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class ParentTransaction 
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ParentId { get; set; }
        public int StudentId { get; set; }

        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }

        //public string PaymentMethod { get; set; }

        [ForeignKey("StudentId")]
        public Student ? Student { get; set; }
        [ForeignKey("ParentId")]
        public Parent ? Parent { get; set; }


    }
}
