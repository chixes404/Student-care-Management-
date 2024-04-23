using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO.AnotherPaymentWay
{
    public class ProcessPaymentRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvc { get; set; }
        public long Amount { get; set; }
        public string ReceiptEmail { get; set; }
        //public string Description { get; set; }
    }
}
