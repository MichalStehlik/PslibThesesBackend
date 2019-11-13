using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;

namespace Authority.Emails.ViewModels
{
    public class MailViewModel
    {
        public string AppUrl { get; set; }
        public string AccentColor { get; set; }

        public MailViewModel()
        {
            AccentColor = "#2196F3";
        }
    }
}
