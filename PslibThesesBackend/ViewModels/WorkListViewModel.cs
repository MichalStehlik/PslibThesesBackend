using PslibThesesBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.ViewModels
{
    public class WorkListViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resources { get; set; }
        public string Subject { get; set; }
        public WorkState State { get; set; }
        [Required]
        public Guid AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorEmail { get; set; }
        public int SetId { get; set; }
        public string SetName { get; set; }
        public DateTime Updated { get; set; }
        public string ClassName { get; set; }
    }
}
