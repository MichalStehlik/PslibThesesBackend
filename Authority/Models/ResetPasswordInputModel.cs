using System.ComponentModel.DataAnnotations;

namespace Authority.Models
{
    public class ResetPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Heslo")]
        [StringLength(100, ErrorMessage = "{0} musí být dlouhé mezi {2} a {1} znaky.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare("Password", ErrorMessage = "Heslo a jeho potvrzení musí být stejné.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
