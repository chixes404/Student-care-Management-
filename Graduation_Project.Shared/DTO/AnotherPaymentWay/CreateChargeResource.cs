using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO.AnotherPaymentWay
{
    public class CreateChargeResource
    {
        public string Currency;
        public long Amount;
        public string CustomerId;
        public string ReceiptEmail;
        public string Description;
    }
}
