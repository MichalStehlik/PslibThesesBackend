﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class WorkRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WorkId { get; set; }
        [ForeignKey("WorkId")]
        [JsonIgnore]
        public Work Work { get; set; }
        [Required]
        public int SetRoleId { get; set; }
        [ForeignKey("RoleId")]
        public SetRole SetRole { get; set; }
        public int? Mark { get; set; }
        public bool Finalized { get; set; } = false;
        public string Review { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Updated { get; set; }
        public ICollection<WorkRoleUser> WorkRoleUsers { get; set; }
    }
}
