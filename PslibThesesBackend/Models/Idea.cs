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
        public List<IdeaGoal> Goals { get; set; }
    }
}
