using Graduation_Project.Shared.Framework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models
{
    public class School : BaseEntity
    {


        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string SchoolType { get; set; }

        public DateTime FoundationDate { get; set; }


        [NotMapped] // Mark as not mapped to database
        public IFormFile ?DbFile { get; set; }
        public bool IsPaid { get; set; }  // Indicates whether the school has completed payment
        public DateTime? PaymentDate { get; set; }  // Date of payment
        public decimal PaymentAmount { get; set; }  // Amount paid by the school  PAYPAL




        public ICollection<Parent> ? Parents { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

        public ICollection<User> ? Users { get; set; }
    }
}
