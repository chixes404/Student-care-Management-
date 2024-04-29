using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class TransactionRequestDto
    {
        public int StudentId { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public List<TransactionProductDto> Products { get; set; }
    }
}
