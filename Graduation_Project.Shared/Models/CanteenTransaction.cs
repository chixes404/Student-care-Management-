using Graduation_Project.Shared.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class CanteenTransaction : BaseEntity
    {

       public int StudentID { get; set; }

        public DateTime TransactionDate;
        public int TransactionAmount { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }



        [ForeignKey("StudentID")]
        public Student Student { get; set; }



    }
}
