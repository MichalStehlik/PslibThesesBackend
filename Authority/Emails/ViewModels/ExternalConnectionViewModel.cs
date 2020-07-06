using Authority.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Emails.ViewModels
{
    public class ExternalConnectionViewModel
    {
        public ApplicationUser User { get; set; }
        public string Provider { get; set; }
        public string ExternalUserId { get; set; }
    }
}
