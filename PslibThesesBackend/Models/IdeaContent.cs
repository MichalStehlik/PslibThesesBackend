using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class IdeaContent
    {
        [Key]
        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Idea { get; set; }
        [Key]
        public int Order { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
