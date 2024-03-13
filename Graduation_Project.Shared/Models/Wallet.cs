using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Shared.Framework;

namespace Graduation_Project.Shared.Models
{
    public class Wallet : BaseEntity
    {
        public decimal Balance { get; set; }  // Current balance in the wallet

        public decimal DailyLimit { get; set; }  // Daily spending limit set by the parent

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [ForeignKey("Parent")]
        public int ParentId { get; set; }

        public Parent Parent { get; set; }


    }
}
