﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class Set
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MaxGrade { get; set; } = 5;
        [Required]
        public bool Active { get; set; } = true;
        [Required]
        public int Year { get; set; }
        [Required]
        public ApplicationTemplate Template { get; set; } = ApplicationTemplate.GraduationWork;
        public ICollection<SetTerm> Terms { get; set; }
        public ICollection<SetRole> Roles { get; set; }
        //public ICollection<Work> Works { get; set; }

    }
}
