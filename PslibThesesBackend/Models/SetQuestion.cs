using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class SetQuestion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        [ForeignKey("SetTermId")]
        public SetTerm Term { get; set; }
        [Required]
        public int SetTermId { get; set; }
        [ForeignKey("SetRoleId")]
        public SetRole Role { get; set; }
        [Required]
        public int SetRoleId { get; set; }
        public ICollection<SetAnswer> Answers { get; set; }
    }
}
