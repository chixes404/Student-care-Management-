using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO.AnotherPaymentWay
{
    public class CustomerResource
    {
        public string CustomerId;
        public string Email;
        public string Name;

        public CustomerResource(string customerId, string email, string name)
        {
            CustomerId = customerId;
            Email = email;
            Name = name;
        }
    }
}
