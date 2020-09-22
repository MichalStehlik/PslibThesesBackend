using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
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
        public bool Manager { get; set; } = false;
        public bool PrintedInApplication { get; set; } = false;
        public bool PrintedInReview { get; set; } = false;
        [JsonIgnore]
        public Set Set { get; set; }
        [Required]
        public int SetId { get; set; }
        [JsonIgnore]
        public ICollection<SetQuestion> Questions { get; set; }
        [JsonIgnore]
        public ICollection<WorkRole> WorkRoles { get; set; }
    }
}
