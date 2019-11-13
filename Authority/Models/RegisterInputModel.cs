using Authority.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class RegisterInputModel : ApplicationUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} musí mít délku mezi {2} a {1} znaky.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Heslo pro potvrzení")]
        [Compare("Password", ErrorMessage = "Heslo a jeho potvrzení musí být stejné.")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
