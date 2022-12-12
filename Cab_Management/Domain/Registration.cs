using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class Registration
    {
        public string? EmailId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Password { get; set; } = null!;

        public int? UserRoleId { get; set; }
        public string? MobileNumber { get; set; }
        public string ConfirmPassword { get; set; } 

    }
}
