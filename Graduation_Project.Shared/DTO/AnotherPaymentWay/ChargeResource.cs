using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO.AnotherPaymentWay
{
    public record ChargeResource(
     string ChargeId,
     string Currency,
     long Amount,
     string CustomerId,
     string ReceiptEmail
     //string Description
        );
}
