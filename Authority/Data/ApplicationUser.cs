using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authority.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Jméno")]
        [PersonalData]
        [Required(ErrorMessage="Jméno musí být vyplněno.")]
        public string FirstName { get; set; }
        [PersonalData]
        [Display(Name = "Prostřední jméno")]
        public string MiddleName { get; set; }
        [PersonalData]
        [Required(ErrorMessage = "Příjmení musí být vyplněno.")]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; }
        [PersonalData]
        [Required(ErrorMessage = "Emailová adresa musí být vyplněna.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public override string Email { get; set; }
        [Display(Name = "Potvrzený Email")]
        public override bool EmailConfirmed { get; set; }
        [Display(Name = "Uživatelské jméno")]
        public override string UserName { get; set; }
        [Display(Name = "Telefonní číslo")]
        [PersonalData]
        public override string PhoneNumber { get; set; }
        [Display(Name = "Potvrzené telefonní číslo")]
        public override bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "Pohlaví")]
        [PersonalData]
        [Required(ErrorMessage = "Pohlaví musí být vyplněno.")]
        public Gender Gender { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + (MiddleName == "" ? "" : (MiddleName + " ")) + " " + LastName;
            }
        }

        public byte[] IconImage { get; set; }
        public string IconImageType { get; set; }
    }
}
