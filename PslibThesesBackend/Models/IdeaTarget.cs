using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class IdeaTarget
    {
        [Key]
        public int IdeaId { get; set; }
        public Idea idea { get; set; }
        [Key]
        public int TargetId { get; set; }
        public Target Target { get; set; }
    }
}
