using PslibThesesBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.ViewModels
{
    public class WorkViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resources { get; set; }
        public string Subject { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public WorkState State { get; set; } = WorkState.InPreparation;
        public int SetId { get; set; }
        public int MaterialCosts { get; set; } = 0;
        public int MaterialCostsProvidedBySchool { get; set; } = 0;
        public int ServicesCosts { get; set; } = 0;
        public int ServicesCostsProvidedBySchool { get; set; } = 0;
        public string DetailExpenditures { get; set; }
        public string ClassName { get; set; }
        public string RepositoryURL { get; set; }
    }
}
