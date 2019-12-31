using PslibThesesBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.ViewModels
{
    public class IdeaListViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resources { get; set; }
        public string Subject { get; set; }
        [Required]
        public string UserId {get; set;}
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public int Participants { get; set; } = 1;
        public bool Offered { get; set; }
        public IEnumerable<Target> Targets { get; set; } = new List<Target>();
        public DateTime Updated { get; set; }
    }
}
