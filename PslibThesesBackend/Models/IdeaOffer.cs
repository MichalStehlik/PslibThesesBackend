using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class IdeaOffer
    {
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
