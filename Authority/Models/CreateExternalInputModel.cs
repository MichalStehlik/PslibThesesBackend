using Authority.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class CreateExternalInputModel : ApplicationUser
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
