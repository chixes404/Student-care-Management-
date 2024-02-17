using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string OldPassword { get; set; }
        public string[] Roles { get; set; }
        public string Email { get; set; }
        public string InsuranceName { get; set; }
        public string PhoneNumber { get; set; }


        //public RegisterRequestDto(string id) { }
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Type { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string Region { get; set; }
        //public string PostalCode { get; set; }

        //public string Country { get; set; }
        //public string Phone { get; set; }
        //public string Fax { get; set; }
        //public string FaxNumber { get; set;
        //public string FaxName { get; set; }
        //public string PhoneNumberNumberNumber { get;set; }

        //public string EmailNumber { get; set; }

    }
}
