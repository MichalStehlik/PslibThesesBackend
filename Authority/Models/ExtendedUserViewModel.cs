using Authority.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class ExtendedUserViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
        public IList<ApplicationRole> AllRoles { get; set; }
        public IList<Claim> Claims { get; set; }
        public IList<UserLoginInfo> External { get; set; }
    }
}
