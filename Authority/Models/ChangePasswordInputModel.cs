using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class ChangePasswordInputModel
    {
        [Required]
        [Display(Name = "Aktuální heslo")]
        [StringLength(100, ErrorMessage = "{0} musí být dlouhé mezi {2} a {1} znaky.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "Heslo")]
        [StringLength(100, ErrorMessage = "{0} musí být dlouhé mezi {2} a {1} znaky.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare("Password", ErrorMessage = "Heslo a jeho potvrzení musí být stejné.")]
        public string ConfirmPassword { get; set; }
    }
}
