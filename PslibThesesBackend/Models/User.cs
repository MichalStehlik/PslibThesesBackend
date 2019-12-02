using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string  MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string AuthorityId { get; set; }
        [Required]
        public string Email { get; set; }
        [DefaultValue(false)]
        public bool CanBeAuthor { get; set; } = false;
        [DefaultValue(false)]
        public bool CanBeEvaluator { get; set; } = false;
        [NotMapped]
        public string Name { get { return FirstName + " " + (String.IsNullOrEmpty(MiddleName) ? "" : (MiddleName + " ")) + LastName; } }
    }
}
