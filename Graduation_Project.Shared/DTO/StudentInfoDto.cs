using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class StudentInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }

        public string GradeTitle { get; set; }
        public string ClassTitle { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal DailyLimit { get; set; }






    }
}
