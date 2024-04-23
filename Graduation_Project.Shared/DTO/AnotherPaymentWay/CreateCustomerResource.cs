using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO.AnotherPaymentWay
{

    public class CreateCustomerResource
    {
        public string Email;
        public string Name;
        public CreateCardResource Card;
    };
}
