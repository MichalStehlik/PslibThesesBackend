using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.ViewModels
{
    public class ApplicationUserViewModel : ApplicationUserInputModel
    {
        public string Id { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        //public string UserName { get; set; }
        public bool HasPassword { get; set; }
        public int AccessFailedCount { get; set; }
    }
}
