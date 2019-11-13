using Authority.Data;

namespace Authority.Emails.ViewModels
{
    public class ConfirmEmailViewModel : MailViewModel
    {
        public string ConfirmationCode { get; set; }
        public ApplicationUser User { get; set; }
        public string ConfirmEmailUrl { get; set; }
    }
}
