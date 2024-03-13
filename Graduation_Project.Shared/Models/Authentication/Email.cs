using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.Models.Authentication
{
    public class ForgetPasswdEmail
    {

        public int Id { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }

        public string Body { get; set; }

    }
}
