using Authority.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class EditUserInputModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; }
        [Display(Name = "Prostřední jméno")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Pohlaví")]
        public Gender Gender { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Uživatelské jméno")]
        public string UserName { get; set; }
        [Display(Name = "Telefonní číslo")]
        public string PhoneNumber { get; set; }
    }
}
