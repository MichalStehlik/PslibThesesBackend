﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class SetTerm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int SetId { get; set; }
        [JsonIgnore]
        public Set Set { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "date")]
        [Required]
        public DateTime WarningDate { get; set; }
        public ICollection<SetQuestion> Questions { get; set; }
    }
}
