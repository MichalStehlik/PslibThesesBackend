using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authority.Data
{
    public class ApplicationRole : IdentityRole
    {
        [Display(Name="Název")]
        public override string Name { get; set; }
    }
}
