using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class ExpandedUserViewModel : PublicUserViewModel
    {
        public List<string> Roles { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
