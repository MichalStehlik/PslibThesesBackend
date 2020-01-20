using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class SetRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool ClassTeacher { get; set; } = false;
        public bool RequiredForPrint { get; set; } = false;
        public bool RequiredForAdvancement { get; set; } = false;
        public bool ShowsOnApplication { get; set; } = false;
        public Set Set { get; set; }
        [Required]
        public int SetId { get; set; }
        public ICollection<SetQuestion> Questions { get; set; }
    }
}
