using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class Work : IdeaFoundation
    {
        public string AuthorId { get; set; }
        [Required]
        [ForeignKey("AuthorId")]
        public User Author { get; set; }
        public string ClassName { get; set; }
        public string ManagerId { get; set; }
        [Required]
        [ForeignKey("ManagerId")]
        public User Manager { get; set; }
        public int SetId { get; set; }
        [Required]
        [ForeignKey("SetId")]
        public Set Set { get; set; }
        public int MaterialCosts { get; set; } = 0;
        public int MaterialCostsBySchool { get; set; } = 0;
        public int ServicesCosts { get; set; } = 0;
        public int ServicesCostsBySchool { get; set; } = 0;
        public string DetailExpenditures { get; set; }
        public WorkState State { get; set; } = WorkState.InPreparation;
        public ICollection<WorkGoal> Goals { get; } = new List<WorkGoal>();
        public ICollection<WorkOutline> Outlines { get; } = new List<WorkOutline>();
    }
}
