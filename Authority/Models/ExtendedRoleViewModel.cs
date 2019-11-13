using Authority.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class ExtendedRoleViewModel
    {
        public ApplicationRole Role { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
