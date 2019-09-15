using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class IdeaGoal
    {
        [Key]
        public Idea Idea { get; set; }
        [Key]
        public int Order { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
