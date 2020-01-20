using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class WorkGoal
    {
        [Key]
        public int Id { get; set; }
        public int WorkId { get; set; }
        [ForeignKey("WorkId")]
        public Idea Work { get; set; }
        public int Order { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
