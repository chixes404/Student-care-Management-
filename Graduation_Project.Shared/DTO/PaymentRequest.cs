using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class PaymentRequest
    {
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }
        public long Amount { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVE must be 3 or 4 digits")]
        public string cvc { get; set; }

        [Required]
        public long ExpiryMonth { get; set; }

        [Required]
        public long ExpiryYear { get; set; }
    }
}
