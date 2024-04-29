using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class CanteenTransactionDto
    {

        public int Id { get; set; }
        [Required]
        [Column(Order = 110)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public DateTime TransactionDate;
        public decimal TransactionAmount { get; set; }

        public string TransactionType { get; set; }

        public string? Description { get; set; }
    }
}
