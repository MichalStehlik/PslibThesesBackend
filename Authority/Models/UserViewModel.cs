using Authority.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Authority.Models
{
    public class UserViewModel : ApplicationUser
    {
        [Display(Name = "Role")]
        public IList<string> Roles { get; set; }
        [Display(Name = "Claimy")]
        public IList<System.Security.Claims.Claim> Claims { get; set; }
        [Display(Name = "Všechny role")]
        public IList<ApplicationRole> AllRoles { get; set; }
    }
}
