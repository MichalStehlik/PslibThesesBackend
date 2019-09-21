using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class Idea : IdeaFoundation
    {
        public int Participants { get; set; }
        public ICollection<IdeaGoal> Goals { get; } = new List<IdeaGoal>();
        public ICollection<IdeaOutline> Outlines { get; } = new List<IdeaOutline>();
        public ICollection<IdeaTarget> Targets { get; } = new List<IdeaTarget>(); 
    }
}
