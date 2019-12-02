using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.ViewModels
{
    public class IdeaViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resources { get; set; }
        public string Subject { get; set; }
        [Required]
        public int UserId { get; set; }
        public int Participants { get; set; } = 1;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Offered { get; set; }
    }
}
