using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.ViewModels
{
    public class PasswordInputModel
    {
        [Required]
        public string Current { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
