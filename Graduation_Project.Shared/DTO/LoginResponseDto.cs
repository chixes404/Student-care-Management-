using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class LoginResponseDto
    {
         public string access_token { get; set; }
        public string token_type { get; set; }
        public Guid user_id { get; set; }  
        public string user_name { get; set; }
    }
}
