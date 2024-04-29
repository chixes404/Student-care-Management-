using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class CanteenTransaction 
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(Order = 110)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        public int StudentID { get; set; }

        public DateTime TransactionDate;
        public decimal TransactionAmount { get; set; }

        public string TransactionType { get; set; }

        public string ?Description { get; set; }
        //[Required]
        //[Column(Order = 120)]
        //public Guid ?CreatedBy { get; set; }


        [ForeignKey("StudentID")]
        [JsonIgnore]

        public Student Student { get; set; }

        // Collection navigation property for products bought in the transaction
        public ICollection<CanteenTransactionProduct> CanteenTransactionProducts { get; set; }



    }
}
